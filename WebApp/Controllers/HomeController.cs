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
    /*
     * metoda calculator i widok w nim napis kalkulator 
     * 
     */

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult About()
    {
        return View();
    }    
    public IActionResult Calculator(Operator? op, decimal? x, decimal? y)
    {
// napisz metode age, parametry : data , result wyswietla wiek, w latach miesiacach i dniach 

        decimal? result = 0.0m;
        if (x is null||y is null)
        {
            ViewBag.ErorMessage = "Niepoprawny format liczby x lub y";
            return View("nullxy");
        }

        if (op is null || (op != Operator.Add && op != Operator.Div && op != Operator.Sub && op!= Operator.Mul))
        {
            ViewBag.ErorMessage("Nieznany operator");
            return View("nullxy");

        }
        
        switch (op)
        {
            case Operator.Add:
                result = x + y;
                ViewBag.Operator = "+";
                break;
            case Operator.Sub:
                result = x - y;
                ViewBag.Operator = "-";
                break;
            case Operator.Mul:
                result = x * y;
                ViewBag.Operator = "*";
                break;
            case Operator.Div:
                result = x / y;
                ViewBag.Operator = "/";
                break;
        }

        ViewBag.Result = result;
        ViewBag.X = x;
        ViewBag.Y = y;
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