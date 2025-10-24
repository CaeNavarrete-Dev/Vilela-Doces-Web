using Microsoft.AspNetCore.Mvc;
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

    public ActionResult Cart()
    {
        return View("Cart");
    }

    public ActionResult Address()
    {
        return View("Address");
    }

    public ActionResult Payment()
    {
        return View("Payment");
    }

    public ActionResult Details()
    {
        return View("Details");
    }

    public ActionResult History()
    {
        return View("History");
    }
}