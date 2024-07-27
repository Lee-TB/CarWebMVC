using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarWebMVC.Models.Domain;
using CarWebMVC.Repositories;

namespace CarWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class EngineTypeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public EngineTypeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: EngineType
    public async Task<IActionResult> Index()
    {
        var engineTypes = await _unitOfWork.EngineTypeRepository.GetAsync();
        return View(engineTypes);
    }

    // GET: EngineType/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var engineType = await _unitOfWork.EngineTypeRepository.GetByIdAsync(id);
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
            _unitOfWork.EngineTypeRepository.Add(engineType);
            await _unitOfWork.SaveChangesAsync();
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

        var engineType = await _unitOfWork.EngineTypeRepository.GetByIdAsync(id);
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
                _unitOfWork.EngineTypeRepository.Update(engineType);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await EngineTypeExists(engineType.Id)))
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

        var engineType = await _unitOfWork.EngineTypeRepository.GetByIdAsync(id);
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
        var engineType = await _unitOfWork.EngineTypeRepository.GetByIdAsync(id);
        if (engineType != null)
        {
            _unitOfWork.EngineTypeRepository.Remove(engineType);
        }

        await _unitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> EngineTypeExists(int id)
    {
        return await _unitOfWork.EngineTypeRepository.ExistsAsync(id);
    }
}
