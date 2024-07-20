using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarWebMVC.Models.Domain;
using CarWebMVC.Repositories;

namespace CarWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class VehicleLineController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public VehicleLineController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: VehicleLine
    public async Task<IActionResult> Index()
    {
        var vehicleLines = await _unitOfWork.VehicleLineRepository.GetAsync(includeProperties: "Manufacturer,VehicleType");
        return View(vehicleLines);
    }

    // GET: VehicleLine/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vehicleLine = await _unitOfWork.VehicleLineRepository
            .GetByIdAsync(id, includeProperties: "Manufacturer,VehicleType");

        if (vehicleLine == null)
        {
            return NotFound();
        }

        return View(vehicleLine);
    }

    // GET: VehicleLine/Create
    public async Task<IActionResult> Create()
    {
        await LoadSelectItemsAsync();
        return View();
    }

    // POST: VehicleLine/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,ManufacturerId,VehicleTypeId")] VehicleLine vehicleLine)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.VehicleLineRepository.Add(vehicleLine);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        await LoadSelectItemsAsync(vehicleLine.ManufacturerId, vehicleLine.VehicleTypeId);
        return View(vehicleLine);
    }

    // GET: VehicleLine/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vehicleLine = await _unitOfWork.VehicleLineRepository.GetByIdAsync(id);
        if (vehicleLine == null)
        {
            return NotFound();
        }
        await LoadSelectItemsAsync(vehicleLine.ManufacturerId, vehicleLine.VehicleTypeId);
        return View(vehicleLine);
    }

    // POST: VehicleLine/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ManufacturerId,VehicleTypeId")] VehicleLine vehicleLine)
    {
        if (id != vehicleLine.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _unitOfWork.VehicleLineRepository.Update(vehicleLine);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await VehicleLineExistsAsync(vehicleLine.Id)))
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
        await LoadSelectItemsAsync(vehicleLine.ManufacturerId, vehicleLine.VehicleTypeId);
        return View(vehicleLine);
    }

    // GET: VehicleLine/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vehicleLine = await _unitOfWork.VehicleLineRepository
            .GetByIdAsync(id, includeProperties: "Manufacturer,VehicleType");
        if (vehicleLine == null)
        {
            return NotFound();
        }

        return View(vehicleLine);
    }

    // POST: VehicleLine/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var vehicleLine = await _unitOfWork.VehicleLineRepository.GetByIdAsync(id);
        if (vehicleLine != null)
        {
            _unitOfWork.VehicleLineRepository.Remove(vehicleLine);
        }

        await _unitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> VehicleLineExistsAsync(int id)
    {
        return await _unitOfWork.VehicleLineRepository.ExistsAsync(id);
    }

    private async Task LoadSelectItemsAsync(object? ManufacturerSelected = null, object? VehicleTypeSelected = null)
    {
        ViewData["ManufacturerSelectItems"] = new SelectList(await _unitOfWork.ManufacturerRepository.GetAsync(), "Id", "Name", ManufacturerSelected);
        ViewData["VehicleTypeSelectItems"] = new SelectList(await _unitOfWork.VehicleTypeRepository.GetAsync(), "Id", "Name", VehicleTypeSelected);
    }
}
