using Microsoft.AspNetCore.Mvc;

namespace VLDocesWeb.Controllers;

public class InitialController : Controller
{
    public ActionResult Index()
    {
        var userName = HttpContext.Session.GetString("UserName");
        var userEmail = HttpContext.Session.GetString("UserEmail");
        ViewBag.userName = userName;
        ViewBag.UserEmail = userEmail;
        return View();
    }
}