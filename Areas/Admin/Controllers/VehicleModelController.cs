using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarWebMVC.Models.Domain;
using CarWebMVC.Models.ViewModels;
using AutoMapper;
using CarWebMVC.Repositories;

namespace CarWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class VehicleModelController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VehicleModelController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // GET: VehicleModel
    public async Task<IActionResult> Index()
    {
        var vehicleModels = await _unitOfWork.VehicleModelRepository.GetAsync(
            includeProperties: "EngineType,Transmission,VehicleLine,Images"
        );

        var vehicleModelViewModelList = vehicleModels
            .Select(vehicleModel => _mapper.Map<VehicleModelViewModel>(vehicleModel))
            .ToList();

        return View(vehicleModelViewModelList);
    }

    // GET: VehicleModel/Details/5
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

    // GET: VehicleModel/Create
    public async Task<IActionResult> Create()
    {
        await LoadSelectListAsync();
        return View();
    }

    // POST: VehicleModel/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Name,Price,Color,InteriorColor,CountryOfOrigin,Year,NumberOfDoors,NumberOfSeats,TransmissionId,EngineTypeId,VehicleLineId")] VehicleModel vehicleModel,
        List<string> newImageUrls)
    {
        if (ModelState.IsValid)
        {
            vehicleModel.Images = newImageUrls.Select(image => new VehicleImage { ImageUrl = image }).ToList();
            _unitOfWork.VehicleModelRepository.Add(vehicleModel);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        await LoadSelectListAsync(
            selectedTransmission: vehicleModel.TransmissionId,
            selectedEngineType: vehicleModel.EngineTypeId,
            selectedVehicleLine: vehicleModel.VehicleLineId
        );
        return View(vehicleModel);
    }

    // GET: VehicleModel/Edit/5
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

    // POST: VehicleModel/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

                    // Add new images to Images tracking collection
                    newImageUrls?.ForEach(imageUrl =>
                    {
                        vehicleModelToUpdate.Images.Add(new VehicleImage { ImageUrl = imageUrl });
                    });
                }

                _unitOfWork.VehicleModelRepository.Update(vehicleModelToUpdate);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _unitOfWork.VehicleModelRepository.ExistsAsync(vehicleModelToUpdate.Id))
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

    // POST: VehicleModel/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var vehicleModel = await _unitOfWork.VehicleModelRepository.GetByIdAsync(id);
        if (vehicleModel != null)
        {
            _unitOfWork.VehicleModelRepository.Remove(vehicleModel);
        }

        await _unitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadSelectListAsync(object? selectedTransmission = null, object? selectedEngineType = null, object? selectedVehicleLine = null)
    {
        ViewBag.TransmissionSelectItems = new SelectList(await _unitOfWork.TransmissionRepository.GetAsync(), "Id", "Name", selectedTransmission);
        ViewBag.EngineTypeSelectItems = new SelectList(await _unitOfWork.EngineTypeRepository.GetAsync(), "Id", "Name", selectedEngineType);
        ViewBag.VehicleLineSelectItems = new SelectList(await _unitOfWork.VehicleLineRepository.GetAsync(), "Id", "Name", selectedVehicleLine);
    }
}
