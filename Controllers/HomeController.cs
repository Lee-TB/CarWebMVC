using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CarWebMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace CarWebMVC.Controllers;

[AllowAnonymous]
[Route("")]
[Route("trang-chu")]
[Route("[controller]")]
public class HomeController : Controller
{
    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("chinh-sach")]
    public IActionResult Privacy()
    {
        return View(nameof(Privacy));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
