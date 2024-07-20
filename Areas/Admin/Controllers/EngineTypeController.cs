using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarWebMVC.Data;
using CarWebMVC.Models.Domain;

namespace CarWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class EngineTypeController : Controller
{
    private readonly AppDbContext _context;

    public EngineTypeController(AppDbContext context)
    {
        _context = context;
    }

    // GET: EngineType
    public async Task<IActionResult> Index()
    {
        return View(await _context.EngineTypes.ToListAsync());
    }

    // GET: EngineType/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var engineType = await _context.EngineTypes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (engineType == null)
        {
            return NotFound();
        }

        return View(engineType);
    }

    // GET: EngineType/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: EngineType/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] EngineType engineType)
    {
        if (ModelState.IsValid)
        {
            _context.Add(engineType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(engineType);
    }

    // GET: EngineType/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var engineType = await _context.EngineTypes.FindAsync(id);
        if (engineType == null)
        {
            return NotFound();
        }
        return View(engineType);
    }

    // POST: EngineType/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] EngineType engineType)
    {
        if (id != engineType.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(engineType);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EngineTypeExists(engineType.Id))
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
        return View(engineType);
    }

    // GET: EngineType/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var engineType = await _context.EngineTypes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (engineType == null)
        {
            return NotFound();
        }

        return View(engineType);
    }

    // POST: EngineType/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var engineType = await _context.EngineTypes.FindAsync(id);
        if (engineType != null)
        {
            _context.EngineTypes.Remove(engineType);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EngineTypeExists(int id)
    {
        return _context.EngineTypes.Any(e => e.Id == id);
    }
}
