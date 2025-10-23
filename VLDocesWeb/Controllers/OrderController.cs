using Microsoft.AspNetCore.Mvc;
namespace VLDocesWeb.Controllers;

public class OrderController : Controller
{
    public ActionResult Index()
    {
        return View();
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