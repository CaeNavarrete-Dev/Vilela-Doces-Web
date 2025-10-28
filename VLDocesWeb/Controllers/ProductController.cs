using VLDocesWeb.Repositories;
using VLDocesWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace VLDocesWeb.Controllers;

public class ProductController : Controller
{
    private IProductRepository repository;
    public ProductController(IProductRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public ActionResult Create()
    {
        return View(new Product());
    }

    [HttpPost]
    public ActionResult Create(Product product)
    {
        repository.Create(product);
        return RedirectToAction("Index", "InitialCo");
    }
}