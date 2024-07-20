using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarWebMVC.Models.Domain;
using CarWebMVC.Repositories;

namespace CarWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class TransmissionController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public TransmissionController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: Transmission
    public async Task<IActionResult> Index()
    {
        var transmissions = await _unitOfWork.TransmissionRepository.GetAsync();
        return View(transmissions);
    }

    // GET: Transmission/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transmission = await _unitOfWork.TransmissionRepository.GetByIdAsync(id);
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
            _unitOfWork.TransmissionRepository.Add(transmission);
            await _unitOfWork.SaveChangesAsync();
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

        var transmission = await _unitOfWork.TransmissionRepository.GetByIdAsync(id);
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
                _unitOfWork.TransmissionRepository.Update(transmission);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await TransmissionExists(transmission.Id)))
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

        var transmission = await _unitOfWork.TransmissionRepository.GetByIdAsync(id);

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
        var transmission = await _unitOfWork.TransmissionRepository.GetByIdAsync(id);
        if (transmission != null)
        {
            _unitOfWork.TransmissionRepository.Remove(transmission);
        }

        await _unitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> TransmissionExists(int id)
    {
        return await _unitOfWork.TransmissionRepository.ExistsAsync(id);
    }
}
