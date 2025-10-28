using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using VLDocesWeb.Models;
using VLDocesWeb.Repositories;

namespace VLDocesWeb.Controllers;

public class AddressController : Controller
{
    private IAddressRepository repository;
    private int usuarioId;

    public AddressController(IAddressRepository repository)
    {
        this.repository = repository;
    }
    
    public ActionResult Index()
    {
        var usuarioId = (int)HttpContext.Session.GetInt32("UserId");
        return View(repository.ReadAll(usuarioId));
    }

    [HttpGet]
    public ActionResult CadastroEnd()
    {
        return View();
    }

    [HttpPost]
    public ActionResult CadastroEnd(Address model)
    {
        var usuarioId = (int)HttpContext.Session.GetInt32("UserId");
        model.ClienteId = usuarioId;

        repository.Create(model);
        return RedirectToAction("Index");
    }
}