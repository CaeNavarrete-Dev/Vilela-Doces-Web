using Microsoft.AspNetCore.Mvc;
using VLDocesWeb.Models;
using VLDocesWeb.Repositories;

namespace VLDocesWeb.Controllers;


public class AccountController : Controller
{
    private ICustomerRepository repository;

    public AccountController(ICustomerRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public ActionResult Login()
    {
        return View(new LoginViewModel());
    }
    [HttpPost]
    public ActionResult Login(LoginViewModel model)
    {
        var login = repository.Login(model);
        if (login == null)
        {
            ViewBag.Message = "Usuario ou senha invalidos";
            return View(model);
        }
        HttpContext.Session.SetString("UserName", login.Nome);
        HttpContext.Session.SetString("UserEmail", login.Email);
        return RedirectToAction("Index","Initial");
    }

    [HttpGet]
    public ActionResult Register()
    {
        return View(new Customer());
    }
    [HttpPost]
    public ActionResult Register(Customer customer)
    {
        Console.WriteLine($"DEBUG => Nome: {customer.Nome}, Email: {customer.Email}, CPF: {customer.CPF}");
        repository.Register(customer);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public ActionResult CadastroEnd()
    {
        return View();
    }
}