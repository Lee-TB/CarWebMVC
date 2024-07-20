using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarWebMVC.Models.Domain;
using CarWebMVC.Repositories;

namespace CarWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class VehicleTypeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public VehicleTypeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: VehicleType
    public async Task<IActionResult> Index()
    {
        var vehicleTypes = await _unitOfWork.VehicleTypeRepository.GetAsync();
        return View(vehicleTypes);
    }

    // GET: VehicleType/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // var vehicleType = await _context.VehicleTypes
        //     .FirstOrDefaultAsync(m => m.Id == id);
        var vehicleType = await _unitOfWork.VehicleTypeRepository.GetByIdAsync(id);
        if (vehicleType == null)
        {
            return NotFound();
        }

        return View(vehicleType);
    }

    // GET: VehicleType/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: VehicleType/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] VehicleType vehicleType)
    {
        if (ModelState.IsValid)
        {
            // _context.Add(vehicleType);
            // await _context.SaveChangesAsync();
            _unitOfWork.VehicleTypeRepository.Add(vehicleType);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(vehicleType);
    }

    // GET: VehicleType/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // var vehicleType = await _context.VehicleTypes.FindAsync(id);
        var vehicleType = await _unitOfWork.VehicleTypeRepository.GetByIdAsync(id);
        if (vehicleType == null)
        {
            return NotFound();
        }
        return View(vehicleType);
    }

    // POST: VehicleType/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] VehicleType vehicleType)
    {
        if (id != vehicleType.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _unitOfWork.VehicleTypeRepository.Update(vehicleType);
                await _unitOfWork.SaveChangesAsync();
                // _context.Update(vehicleType);
                // await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await VehicleTypeExists(vehicleType.Id)))
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
        return View(vehicleType);
    }

    // GET: VehicleType/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // var vehicleType = await _context.VehicleTypes
        //     .FirstOrDefaultAsync(m => m.Id == id);
        var vehicleType = await _unitOfWork.VehicleTypeRepository.GetByIdAsync(id);
        if (vehicleType == null)
        {
            return NotFound();
        }

        return View(vehicleType);
    }

    // POST: VehicleType/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // var vehicleType = await _context.VehicleTypes.FindAsync(id);
        var vehicleType = await _unitOfWork.VehicleTypeRepository.GetByIdAsync(id);
        if (vehicleType != null)
        {
            // _context.VehicleTypes.Remove(vehicleType);
            _unitOfWork.VehicleTypeRepository.Remove(vehicleType);
        }

        // await _context.SaveChangesAsync();
        await _unitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> VehicleTypeExists(int id)
    {
        return await _unitOfWork.VehicleTypeRepository.ExistsAsync(id);
    }
}
