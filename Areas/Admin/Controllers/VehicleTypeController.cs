using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarWebMVC.Data;
using CarWebMVC.Models;

namespace CarWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class VehicleTypeController : Controller
{
    private readonly AppDbContext _context;

    public VehicleTypeController(AppDbContext context)
    {
        _context = context;
    }

    // GET: VehicleType
    public async Task<IActionResult> Index()
    {
        return View(await _context.VehicleTypes.ToListAsync());
    }

    // GET: VehicleType/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vehicleType = await _context.VehicleTypes
            .FirstOrDefaultAsync(m => m.Id == id);
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
            _context.Add(vehicleType);
            await _context.SaveChangesAsync();
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

        var vehicleType = await _context.VehicleTypes.FindAsync(id);
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
                _context.Update(vehicleType);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleTypeExists(vehicleType.Id))
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

        var vehicleType = await _context.VehicleTypes
            .FirstOrDefaultAsync(m => m.Id == id);
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
        var vehicleType = await _context.VehicleTypes.FindAsync(id);
        if (vehicleType != null)
        {
            _context.VehicleTypes.Remove(vehicleType);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool VehicleTypeExists(int id)
    {
        return _context.VehicleTypes.Any(e => e.Id == id);
    }
}
