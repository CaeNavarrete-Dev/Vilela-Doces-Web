using Microsoft.AspNetCore.Mvc;

namespace VLDocesWeb.Controllers;

public class InitialController : Controller
{
    public ActionResult Index()
    {
        return View();
    }
}