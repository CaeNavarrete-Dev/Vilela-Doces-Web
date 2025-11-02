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
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Address model)
    {
        var usuarioId = (int)HttpContext.Session.GetInt32("UserId");
        model.ClienteId = usuarioId;

        repository.Create(model);
        return RedirectToAction("Index");
    }

    public ActionResult Delete(int endId)
    {
        repository.Delete(endId);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Update(int endId)
    {
        var Address = repository.Read(endId);
        return View(Address);
    }

    [HttpPost]
    public ActionResult Update(int id, Address model)
    {
        model.AddressId = id;
        model.ClienteId = (int)HttpContext.Session.GetInt32("UserId");
        repository.Update(model);

        return RedirectToAction("Index");
    }
}