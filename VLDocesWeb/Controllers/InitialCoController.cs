using VLDocesWeb.Repositories;
using VLDocesWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace VLDocesWeb.Controllers;

public class InitialCoController : Controller
{
    public ActionResult Index()
    {
        var userType = HttpContext.Session.GetString("UserType");
        if (userType != "Collaborator")
        {
            return RedirectToAction("Login", "Account");
        }
        var userName = HttpContext.Session.GetString("UserName");
        var userEmail = HttpContext.Session.GetString("UserEmail");
        var userTelefone = HttpContext.Session.GetString("UserTelefone");
        ViewBag.userName = userName;
        ViewBag.userEmail = userEmail;
        ViewBag.userTelefone = userTelefone;

        return View();
    }
}