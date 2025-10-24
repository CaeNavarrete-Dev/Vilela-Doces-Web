using Microsoft.AspNetCore.Mvc;

namespace VLDocesWeb.Controllers;

public class InitialController : Controller
{
    public ActionResult Index()
    {
        var userName = HttpContext.Session.GetString("UserName");
        var userEmail = HttpContext.Session.GetString("UserEmail");
        var userCPF = HttpContext.Session.GetString("UserCPF");
        var userTelefone = HttpContext.Session.GetString("UserTelefone");
        ViewBag.userName = userName;
        ViewBag.UserEmail = userEmail;
        ViewBag.userCPF = userCPF;
        ViewBag.userTelefone = userTelefone;
        return View();
    }
}