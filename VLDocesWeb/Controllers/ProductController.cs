using VLDocesWeb.Repositories;
using VLDocesWeb.Models;
using Microsoft.AspNetCore.Mvc;

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
        return RedirectToAction("ListAll", "Product");
    }

    [HttpGet]
    public ActionResult ListAll()
    {
        List<Product> _products = repository.ListAll();
        return View(_products);
    }

    [HttpPost]
    public ActionResult Delete(int id)
    {
        repository.Delete(id);
        return RedirectToAction("ListAll", "Product");
    }

    [HttpGet]
    public ActionResult Update(int id)
    {
        Product product = repository.Read(id);
        return View(product);
    }
    [HttpPost]
    public ActionResult Update(int id, Product product)
    {
        product.Id_Produto = id;
        repository.Update(product);
        
        return RedirectToAction("ListAll", "Product");
    }
}