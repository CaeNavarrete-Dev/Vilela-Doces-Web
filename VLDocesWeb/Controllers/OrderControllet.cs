using Microsoft.AspNetCore.Mvc;
using VLDocesWeb.Repositories;
namespace VLDocesWeb.Controllers;

public class OrderController : Controller
{
    private IProductRepository repository;
    public OrderController(IProductRepository repository)
    {
        this.repository = repository;
    }
    public ActionResult Index()
    {
        var _products = repository.ListAll();
        return View(_products);
    }
}