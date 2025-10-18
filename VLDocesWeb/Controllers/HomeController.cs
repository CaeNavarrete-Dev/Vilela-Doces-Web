using Microsoft.AspNetCore.Mvc;


namespace VLDocesWeb.Controllers;

public class HomeController : Controller
{
    public ActionResult Index()
    {
        return View();
    }
}