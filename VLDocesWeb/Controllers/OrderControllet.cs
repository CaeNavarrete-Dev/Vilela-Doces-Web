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
}