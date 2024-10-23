using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class ContactController : Controller
{
    private static Dictionary<int, ContactModel> _contacts = new()
    {
        {
            1, new ContactModel()
            {
                Id = 1, First_name = "michal", Last_name = "krul", Email = "jestemkrulem@gmail.com", Phonenumber = "999888777", Date_of_Birth = new DateOnly(2000, 10, 30)
            }
        },
        {
        2, new ContactModel()
        {
            Id = 2, First_name = "michal", Last_name = "krol", Email = "niejestemkrulem@gmail.com", Phonenumber = "111222333", Date_of_Birth = new DateOnly(2002, 10, 30)
        }}
    };

    private static int _currentId = 2;
    // Lista kontaków
    public IActionResult Index()
    {
        return View(_contacts);
    }
    // Formularz dodawania
    [HttpGet]
    public IActionResult Add()
    {
        return View("AddForm");
    }
    // odebranie danych i zapis kontaku
    [HttpPost]
    public IActionResult Add(ContactModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("AddForm");
        }

        model.Id = ++_currentId;
        _contacts.Add(model.Id, model);
        // wyswietlanie danych
        return View("Index", _contacts);
    }

    public IActionResult Delete(int id)
    {
        _contacts.Remove(id);
        return View("Index", _contacts);
    }

    public IActionResult Details(int id)
    {
        return View(_contacts[id]);
    }
}