using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarWebMVC.Models.Domain;
using CarWebMVC.Models.ViewModels;
using AutoMapper;
using CarWebMVC.Repositories;
using CarWebMVC.Models;
using CarWebMVC.Services;
using CarWebMVC.Models.LuceneDTO;

namespace CarWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class VehicleModelController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILuceneService<VehicleModelLuceneDTO> _luceneService;

    public VehicleModelController(IUnitOfWork unitOfWork, IMapper mapper, ILuceneService<VehicleModelLuceneDTO> luceneService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _luceneService = luceneService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search, int pageIndex = 1, int pageSize = 10)
    {
        ViewBag.currentSearch = search;
        ViewBag.currentPageSize = pageSize;

        PaginatedList<VehicleModel>? vehicleModels;

        if (string.IsNullOrWhiteSpace(search))
        {
            vehicleModels = await _unitOfWork.VehicleModelRepository.GetPaginatedAsync(
                includeProperties: "EngineType,Transmission,VehicleLine,Images",
                pageIndex: pageIndex,
                pageSize: pageSize
            );
        }
        else
        {
            // Search in Lucene index and get list of vehicle model from database that match the search query in Lucene index
            var searchResults = _luceneService.Search<VehicleModelLuceneDTO>(search);
            var listId = searchResults.Select(result => result.Id).ToList();
            vehicleModels = await _unitOfWork.VehicleModelRepository.GetPaginatedAsync(
                includeProperties: "EngineType,Transmission,VehicleLine,Images",
                filter: vehicleModel => listId.Contains(vehicleModel.Id),
                pageIndex: pageIndex,
                pageSize: pageSize
            );
        }

        var viewModels = vehicleModels
            .Select(vehicleModel => _mapper.Map<VehicleModelViewModel>(vehicleModel)).ToList();

        var vehicleModelViewModelPaginatedList = new PaginatedList<VehicleModelViewModel>(viewModels,
            vehicleModels.PageIndex,
            vehicleModels.TotalPages);

        return View(vehicleModelViewModelPaginatedList);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vehicleModel = await _unitOfWork.VehicleModelRepository
            .GetByIdAsync(id, includeProperties: "EngineType,Transmission,VehicleLine,Images");
        if (vehicleModel == null)
        {
            return NotFound();
        }

        var vehicleModelViewModel = _mapper.Map<VehicleModelViewModel>(vehicleModel);
        return View(vehicleModelViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadSelectListAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Name,Price,Color,InteriorColor,CountryOfOrigin,Year,NumberOfDoors,NumberOfSeats,TransmissionId,EngineTypeId,VehicleLineId")] VehicleModel vehicleModel,
        List<string> newImageUrls)
    {
        if (ModelState.IsValid)
        {
            // Save to database
            vehicleModel.Images = newImageUrls.Select(image => new VehicleImage { ImageUrl = image }).ToList();
            _unitOfWork.VehicleModelRepository.Add(vehicleModel);
            await _unitOfWork.SaveChangesAsync();

            // Save to Lucene index
            var vehicleModelLuceneDTO = _mapper.Map<VehicleModelLuceneDTO>(vehicleModel);
            var vehicleLine = await _unitOfWork.VehicleLineRepository.GetByIdAsync(vehicleModel.VehicleLineId);
            vehicleModelLuceneDTO.VehicleLineName = vehicleLine?.Name ?? "";
            var engineType = await _unitOfWork.EngineTypeRepository.GetByIdAsync(vehicleModel.EngineTypeId);
            vehicleModelLuceneDTO.EngineTypeName = engineType?.Name ?? "";
            var transmission = await _unitOfWork.TransmissionRepository.GetByIdAsync(vehicleModel.TransmissionId);
            vehicleModelLuceneDTO.TransmissionName = transmission?.Name ?? "";
            _luceneService.Add(vehicleModelLuceneDTO);
            _luceneService.Commit();

            return RedirectToAction(nameof(Index));
        }

        await LoadSelectListAsync(
            selectedTransmission: vehicleModel.TransmissionId,
            selectedEngineType: vehicleModel.EngineTypeId,
            selectedVehicleLine: vehicleModel.VehicleLineId
        );
        return View(vehicleModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vehicleModel = await _unitOfWork.VehicleModelRepository
            .GetByIdAsync(id, includeProperties: "EngineType,Transmission,VehicleLine,Images");

        if (vehicleModel == null)
        {
            return NotFound();
        }

        var vehicleModelViewModel = _mapper.Map<VehicleModelViewModel>(vehicleModel);

        await LoadSelectListAsync(
            selectedTransmission: vehicleModel.TransmissionId,
            selectedEngineType: vehicleModel.EngineTypeId,
            selectedVehicleLine: vehicleModel.VehicleLineId
        );
        return View(vehicleModelViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Name,Price,Color,InteriorColor,CountryOfOrigin,Year,NumberOfDoors,NumberOfSeats,TransmissionId,EngineTypeId,VehicleLineId")] VehicleModel vehicleModelToUpdate,
        List<string> newImageUrls,
        List<string> existingImageUrls)
    {
        if (id != vehicleModelToUpdate.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _unitOfWork.VehicleModelRepository.LoadCollectionAsync(vehicleModelToUpdate, "Images");
                if (vehicleModelToUpdate.Images?.Count > 0)
                {
                    // Remove all images which not checked in the form from the database
                    var imagesToRemove = vehicleModelToUpdate.Images.Where(image => !existingImageUrls.Contains(image.ImageUrl)).ToList();
                    _unitOfWork.VehicleImageRepository.RemoveRange(imagesToRemove);
                }
                else
                {
                    // if there is no image in the database, create a new list of images
                    vehicleModelToUpdate.Images = new List<VehicleImage>();
                }
                // Add new images to Images tracking collection                
                newImageUrls?.ForEach(imageUrl =>
                {
                    vehicleModelToUpdate.Images.Add(new VehicleImage { ImageUrl = imageUrl });
                });

                // Update vehicle model in the database
                _unitOfWork.VehicleModelRepository.Update(vehicleModelToUpdate);
                await _unitOfWork.SaveChangesAsync();

                // Update vehicle model in the Lucene index
                var vehicleModelLuceneDTO = _mapper.Map<VehicleModelLuceneDTO>(vehicleModelToUpdate);
                var vehicleLine = await _unitOfWork.VehicleLineRepository.GetByIdAsync(vehicleModelToUpdate.VehicleLineId);
                vehicleModelLuceneDTO.VehicleLineName = vehicleLine?.Name ?? "";
                var engineType = await _unitOfWork.EngineTypeRepository.GetByIdAsync(vehicleModelToUpdate.EngineTypeId);
                vehicleModelLuceneDTO.EngineTypeName = engineType?.Name ?? "";
                var transmission = await _unitOfWork.TransmissionRepository.GetByIdAsync(vehicleModelToUpdate.TransmissionId);
                vehicleModelLuceneDTO.TransmissionName = transmission?.Name ?? "";
                _luceneService.Update(vehicleModelLuceneDTO);
                _luceneService.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await _unitOfWork.VehicleModelRepository.ExistsAsync(vehicleModelToUpdate.Id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        await LoadSelectListAsync(
            selectedTransmission: vehicleModelToUpdate.TransmissionId,
            selectedEngineType: vehicleModelToUpdate.EngineTypeId,
            selectedVehicleLine: vehicleModelToUpdate.VehicleLineId
        );

        var vehicleModelViewModel = _mapper.Map<VehicleModelViewModel>(vehicleModelToUpdate);

        return View(vehicleModelViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var vehicleModel = await _unitOfWork.VehicleModelRepository.GetByIdAsync(id);
        if (vehicleModel != null)
        {
            // Remove from database
            _unitOfWork.VehicleModelRepository.Remove(vehicleModel);
            await _unitOfWork.SaveChangesAsync();
            // Remove from Lucene index
            _luceneService.Delete(new VehicleModelLuceneDTO { Id = vehicleModel.Id });
            _luceneService.Commit();
        }

        return RedirectToAction(nameof(Index));
    }

    // Use this action to build the Lucene index manually
    [HttpGet]
    public async Task<string> BuildIndex()
    {
        var vehicleModels = await _unitOfWork.VehicleModelRepository.GetAsync(includeProperties: "EngineType,Transmission,VehicleLine,Images");
        var vehicleModelLuceneDTOs = vehicleModels.Select(vehicleModel => _mapper.Map<VehicleModelLuceneDTO>(vehicleModel));
        _luceneService.AddRange(vehicleModelLuceneDTOs);
        _luceneService.Commit();
        return "Vehicle Model Index built";
    }

    // Use this action to clear the Lucene index manually
    [HttpGet]
    public string ClearIndex()
    {
        _luceneService.Clear();
        _luceneService.Commit();
        return "Vehicle Model Index cleared";
    }

    private async Task LoadSelectListAsync(object? selectedTransmission = null, object? selectedEngineType = null, object? selectedVehicleLine = null)
    {
        ViewBag.TransmissionSelectItems = new SelectList(await _unitOfWork.TransmissionRepository.GetAsync(), "Id", "Name", selectedTransmission);
        ViewBag.EngineTypeSelectItems = new SelectList(await _unitOfWork.EngineTypeRepository.GetAsync(), "Id", "Name", selectedEngineType);
        ViewBag.VehicleLineSelectItems = new SelectList(await _unitOfWork.VehicleLineRepository.GetAsync(), "Id", "Name", selectedVehicleLine);
    }
}
