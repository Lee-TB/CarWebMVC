using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarWebMVC.Data;
using CarWebMVC.Models.Domain;

namespace CarWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class TransmissionController : Controller
{
    private readonly AppDbContext _context;

    public TransmissionController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Transmission
    public async Task<IActionResult> Index()
    {
        return View(await _context.Transmission.ToListAsync());
    }

    // GET: Transmission/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transmission = await _context.Transmission
            .FirstOrDefaultAsync(m => m.Id == id);
        if (transmission == null)
        {
            return NotFound();
        }

        return View(transmission);
    }

    // GET: Transmission/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Transmission/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description")] Transmission transmission)
    {
        if (ModelState.IsValid)
        {
            _context.Add(transmission);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(transmission);
    }

    // GET: Transmission/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transmission = await _context.Transmission.FindAsync(id);
        if (transmission == null)
        {
            return NotFound();
        }
        return View(transmission);
    }

    // POST: Transmission/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Transmission transmission)
    {
        if (id != transmission.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(transmission);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransmissionExists(transmission.Id))
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
        return View(transmission);
    }

    // GET: Transmission/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transmission = await _context.Transmission
            .FirstOrDefaultAsync(m => m.Id == id);
        if (transmission == null)
        {
            return NotFound();
        }

        return View(transmission);
    }

    // POST: Transmission/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var transmission = await _context.Transmission.FindAsync(id);
        if (transmission != null)
        {
            _context.Transmission.Remove(transmission);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TransmissionExists(int id)
    {
        return _context.Transmission.Any(e => e.Id == id);
    }
}
