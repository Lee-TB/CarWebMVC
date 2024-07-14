using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarWebMVC.Data;
using CarWebMVC.Models;
using CarWebMVC.Models.ViewModels;
using AutoMapper;

namespace CarWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class VehicleModelController : Controller
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public VehicleModelController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: VehicleModel
    public async Task<IActionResult> Index()
    {
        var vehicleModels = await _context.VehicleModels
            .Include(v => v.EngineType)
            .Include(v => v.Transmission)
            .Include(v => v.VehicleLine)
            .Include(v => v.Images).ToListAsync();

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

        var vehicleModel = await _context.VehicleModels
            .Include(v => v.EngineType)
            .Include(v => v.Transmission)
            .Include(v => v.VehicleLine)
            .Include(v => v.Images)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (vehicleModel == null)
        {
            return NotFound();
        }

        var vehicleModelViewModel = _mapper.Map<VehicleModelViewModel>(vehicleModel);

        return View(vehicleModelViewModel);
    }

    // GET: VehicleModel/Create
    public IActionResult Create()
    {
        LoadSelectList();
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
            _context.Add(vehicleModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        LoadSelectList(
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

        var vehicleModel = await _context.VehicleModels
            .Include(v => v.EngineType)
            .Include(v => v.Transmission)
            .Include(v => v.VehicleLine)
            .Include(v => v.Images)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (vehicleModel == null)
        {
            return NotFound();
        }

        var vehicleModelViewModel = _mapper.Map<VehicleModelViewModel>(vehicleModel);

        LoadSelectList(
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
                _context.Entry(vehicleModelToUpdate).Collection(v => v.Images).Load();
                if (vehicleModelToUpdate.Images?.Count > 0)
                {
                    // Remove all images which not checked in the form from the database
                    var imagesToRemove = vehicleModelToUpdate.Images.Where(image => !existingImageUrls.Contains(image.ImageUrl)).ToList();
                    _context.VehicleImages.RemoveRange(imagesToRemove);

                    // Add new images to Images tracking collection
                    newImageUrls?.ForEach(imageUrl =>
                    {
                        vehicleModelToUpdate.Images.Add(new VehicleImage { ImageUrl = imageUrl });
                    });
                }

                _context.Update(vehicleModelToUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleModelExists(vehicleModelToUpdate.Id))
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
        LoadSelectList(
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
        var vehicleModel = await _context.VehicleModels.FindAsync(id);
        if (vehicleModel != null)
        {
            _context.VehicleModels.Remove(vehicleModel);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool VehicleModelExists(int id)
    {
        return _context.VehicleModels.Any(e => e.Id == id);
    }

    private void LoadSelectList(object? selectedTransmission = null, object? selectedEngineType = null, object? selectedVehicleLine = null)
    {
        ViewBag.TransmissionSelectItems = new SelectList(_context.Transmission, "Id", "Name", selectedTransmission);
        ViewBag.EngineTypeSelectItems = new SelectList(_context.EngineTypes, "Id", "Name", selectedEngineType);
        ViewBag.VehicleLineSelectItems = new SelectList(_context.VehicleLines, "Id", "Name", selectedVehicleLine);
    }
}
