using Microsoft.AspNetCore.Mvc;

namespace CarWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class ManufacturerController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Edit(int id)
    {
        return View();
    }

    public IActionResult Delete(int id)
    {
        return View();
    }

    public IActionResult Details(int id)
    {
        return View();
    }
}