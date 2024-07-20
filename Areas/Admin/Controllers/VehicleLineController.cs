using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarWebMVC.Data;
using CarWebMVC.Models.Domain;

namespace CarWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class VehicleLineController : Controller
{
    private readonly AppDbContext _context;

    public VehicleLineController(AppDbContext context)
    {
        _context = context;
    }

    // GET: VehicleLine
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.VehicleLines.Include(v => v.Manufacturer).Include(v => v.VehicleType);
        return View(await appDbContext.ToListAsync());
    }

    // GET: VehicleLine/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vehicleLine = await _context.VehicleLines
            .Include(v => v.Manufacturer)
            .Include(v => v.VehicleType)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (vehicleLine == null)
        {
            return NotFound();
        }

        return View(vehicleLine);
    }

    // GET: VehicleLine/Create
    public IActionResult Create()
    {
        LoadSelectItems();
        return View();
    }

    // POST: VehicleLine/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,ManufacturerId,VehicleTypeId")] VehicleLine vehicleLine)
    {
        PrintModelStateErrors();
        if (ModelState.IsValid)
        {
            _context.Add(vehicleLine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        LoadSelectItems(vehicleLine.ManufacturerId, vehicleLine.VehicleTypeId);
        return View(vehicleLine);
    }

    // GET: VehicleLine/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vehicleLine = await _context.VehicleLines.FindAsync(id);
        if (vehicleLine == null)
        {
            return NotFound();
        }
        LoadSelectItems(vehicleLine.ManufacturerId, vehicleLine.VehicleTypeId);
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
                _context.Update(vehicleLine);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleLineExists(vehicleLine.Id))
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
        LoadSelectItems(vehicleLine.ManufacturerId, vehicleLine.VehicleTypeId);
        return View(vehicleLine);
    }

    // GET: VehicleLine/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vehicleLine = await _context.VehicleLines
            .Include(v => v.Manufacturer)
            .Include(v => v.VehicleType)
            .FirstOrDefaultAsync(m => m.Id == id);
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
        var vehicleLine = await _context.VehicleLines.FindAsync(id);
        if (vehicleLine != null)
        {
            _context.VehicleLines.Remove(vehicleLine);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool VehicleLineExists(int id)
    {
        return _context.VehicleLines.Any(e => e.Id == id);
    }

    private void LoadSelectItems(object? ManufacturerSelected = null, object? VehicleTypeSelected = null)
    {
        ViewData["ManufacturerSelectItems"] = new SelectList(_context.Manufacturers, "Id", "Name", ManufacturerSelected);
        ViewData["VehicleTypeSelectItems"] = new SelectList(_context.VehicleTypes, "Id", "Name", VehicleTypeSelected);
    }

    private void PrintModelStateErrors()
    {
        foreach (var modelState in ModelState.Values)
        {
            foreach (var modelError in modelState.Errors)
            {
                Console.WriteLine(modelError.ErrorMessage);
            }
        }
    }
}
