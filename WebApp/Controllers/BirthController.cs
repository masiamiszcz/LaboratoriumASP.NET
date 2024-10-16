using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class BirthController : Controller
    {
        public IActionResult BirthForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Result(Birth birth)
        {
            if (!birth.IsValid())
            {
                ViewBag.ErrorMessage = "Proszę podać poprawne imię i datę urodzin.";
                return View("BirthForm");
            }

            int age = birth.CalculateAge();
            ViewBag.Message = $"Cześć {birth.Name}, masz {age} lat(a).";

            return View("BirthResult");
        }
    }
}