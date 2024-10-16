using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult About()
    {
        return View();
    }    
    public IActionResult Age(DateTime birthDate)
    {
        var today = DateTime.Today;
        int years = today.Year - birthDate.Year;

        if (birthDate.Date > today.AddYears(-years)) 
        {
            years--;
        }

        int months = today.Month - birthDate.Month;
        if (months < 0) 
        {
            months += 12;
        }

        int days = today.Day - birthDate.Day;
        if (days < 0) 
        {
            days += DateTime.DaysInMonth(today.Year, today.Month - 1);
        }

        ViewBag.Age = $"{years} lat, {months} miesięcy i {days} dni";
        return View("ageView"); 
    }
    public IActionResult Calculator()
    {
        return View();
    }

    
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public enum Operator
    {
        Add, Sub, Mul, Div
    }
}