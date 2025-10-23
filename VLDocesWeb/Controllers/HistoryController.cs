using Microsoft.AspNetCore.Mvc;
namespace VLDocesWeb.Controllers;

public class HistoryController : Controller
{
    public ActionResult Index()
    {
        return View();
    }
}