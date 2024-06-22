using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarWebMVC.Data;
using CarWebMVC.Models;

namespace CarWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class VehicleModelController : Controller
{
    private readonly AppDbContext _context;

    public VehicleModelController(AppDbContext context)
    {
        _context = context;
    }

    // GET: VehicleModel
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.VehicleModels.Include(v => v.EngineType).Include(v => v.Transmission).Include(v => v.VehicleLine);
        return View(await appDbContext.ToListAsync());
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
            .FirstOrDefaultAsync(m => m.Id == id);
        if (vehicleModel == null)
        {
            return NotFound();
        }

        return View(vehicleModel);
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
    public async Task<IActionResult> Create([Bind("Id,Name,Price,Color,InteriorColor,CountryOfOrigin,Year,NumberOfDoors,NumberOfSeats,TransmissionId,EngineTypeId,VehicleLineId")] VehicleModel vehicleModel)
    {
        if (ModelState.IsValid)
        {
            _context.Add(vehicleModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["EngineTypeId"] = new SelectList(_context.EngineTypes, "Id", "Id", vehicleModel.EngineTypeId);
        ViewData["TransmissionId"] = new SelectList(_context.Transmission, "Id", "Id", vehicleModel.TransmissionId);
        ViewData["VehicleLineId"] = new SelectList(_context.VehicleLines, "Id", "Id", vehicleModel.VehicleLineId);
        return View(vehicleModel);
    }

    // GET: VehicleModel/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vehicleModel = await _context.VehicleModels.FindAsync(id);
        if (vehicleModel == null)
        {
            return NotFound();
        }
        LoadSelectList(
            selectedTransmission: vehicleModel.TransmissionId,
            selectedEngineType: vehicleModel.EngineTypeId,
            selectedVehicleLine: vehicleModel.VehicleLineId
        );
        return View(vehicleModel);
    }

    // POST: VehicleModel/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Color,InteriorColor,CountryOfOrigin,Year,NumberOfDoors,NumberOfSeats,TransmissionId,EngineTypeId,VehicleLineId")] VehicleModel vehicleModel)
    {
        if (id != vehicleModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(vehicleModel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleModelExists(vehicleModel.Id))
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
            selectedTransmission: vehicleModel.TransmissionId,
            selectedEngineType: vehicleModel.EngineTypeId,
            selectedVehicleLine: vehicleModel.VehicleLineId
        );
        return View(vehicleModel);
    }

    // GET: VehicleModel/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vehicleModel = await _context.VehicleModels
            .Include(v => v.EngineType)
            .Include(v => v.Transmission)
            .Include(v => v.VehicleLine)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (vehicleModel == null)
        {
            return NotFound();
        }

        return View(vehicleModel);
    }

    // POST: VehicleModel/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
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
