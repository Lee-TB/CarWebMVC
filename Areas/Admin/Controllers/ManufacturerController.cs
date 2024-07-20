using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarWebMVC.Models.Domain;
using CarWebMVC.Repositories;

namespace CarWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class ManufacturerController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public ManufacturerController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: Manufacturer
    public async Task<IActionResult> Index()
    {
        var manufacturers = await _unitOfWork.ManufacturerRepository.GetAsync();
        return View(manufacturers);
    }

    // GET: Manufacturer/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var manufacturer = await _unitOfWork.ManufacturerRepository.GetByIdAsync(id);
        if (manufacturer == null)
        {
            return NotFound();
        }

        return View(manufacturer);
    }

    // GET: Manufacturer/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Manufacturer/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Country,FoundedYear")] Manufacturer manufacturer)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.ManufacturerRepository.Add(manufacturer);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(manufacturer);
    }

    // GET: Manufacturer/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var manufacturer = await _unitOfWork.ManufacturerRepository.GetByIdAsync(id);
        if (manufacturer == null)
        {
            return NotFound();
        }
        return View(manufacturer);
    }

    // POST: Manufacturer/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Country,FoundedYear")] Manufacturer manufacturer)
    {
        if (id != manufacturer.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _unitOfWork.ManufacturerRepository.Update(manufacturer);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await ManufacturerExists(manufacturer.Id)))
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
        return View(manufacturer);
    }

    // GET: Manufacturer/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var manufacturer = await _unitOfWork.ManufacturerRepository.GetByIdAsync(id);
        if (manufacturer == null)
        {
            return NotFound();
        }

        return View(manufacturer);
    }

    // POST: Manufacturer/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var manufacturer = await _unitOfWork.ManufacturerRepository.GetByIdAsync(id);
        if (manufacturer != null)
        {
            _unitOfWork.ManufacturerRepository.Remove(manufacturer);
        }

        await _unitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> ManufacturerExists(int id)
    {
        return await _unitOfWork.ManufacturerRepository.ExistsAsync(id);
    }
}
