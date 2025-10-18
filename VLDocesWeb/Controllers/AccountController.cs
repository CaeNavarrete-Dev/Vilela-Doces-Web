using Microsoft.AspNetCore.Mvc;

namespace VLDocesWeb.Controllers;


public class AccountController : Controller
{
    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    [HttpGet]
    public ActionResult Register()
    {
        return View();
    }
}