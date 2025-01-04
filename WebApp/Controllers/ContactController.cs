using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApp.Models;

using WebApp.Models.Services;

namespace WebApp.Controllers;
[Authorize(Roles = "admin")]
public class ContactController : Controller
{
    private readonly IContactService _contactService;
    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }
    [AllowAnonymous]
    // Lista kontaków
    public IActionResult Index()
    {
        return View( _contactService.GetAll());
    }
    // Formularz dodawania
    [HttpGet]
    public IActionResult Add()
    {
        var model = new ContactModel();
        model.Organizations = _contactService.GetAllOrganizaions()
            .Select(e => new SelectListItem()
                {
                Value = e.Id.ToString(),
                Text = e.Name,
                Selected = e.Id==102
                }
            ).ToList();
        return View(model);
    }
    // odebranie danych i zapis kontaku
    [HttpPost]
    public IActionResult Add(ContactModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Organizations = _contactService.GetAllOrganizaions()
                .Select(e => new SelectListItem()
                    {
                        Value = e.Id.ToString(),
                        Text = e.Name,
                        Selected = e.Id==model.Id
                    }
                ).ToList();
            return View(model);
        }
        _contactService.Add(model);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        _contactService.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Details(int id)
    {
        return View(_contactService.GetById(id));
    }
    public IActionResult Edit(int id)
    {
        return View(_contactService.GetById(id));
    }
    [HttpPost]
    public IActionResult Edit(ContactModel model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        _contactService.Update(model);
        return RedirectToAction(nameof(Index));
    }
}