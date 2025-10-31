using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
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
        else if (login is Customer customer)
        {
            HttpContext.Session.SetInt32("UderId", customer.Id);
            HttpContext.Session.SetString("UserName", customer.Nome);
            HttpContext.Session.SetString("UserEmail", customer.Email);
            HttpContext.Session.SetString("UserCPF", customer.CPF);
            HttpContext.Session.SetString("UserTelefone", customer.Telefone);
            HttpContext.Session.SetString("UserType", "Customer");
            return RedirectToAction("Index", "Initial");
        }
        else
        {
            HttpContext.Session.SetInt32("UserId", login.Id);
            HttpContext.Session.SetString("UserName", login.Nome);
            HttpContext.Session.SetString("UserEmail", login.Email);
            HttpContext.Session.SetString("UserTelefone", login.Telefone);
            HttpContext.Session.SetString("UserType", "Collaborator");
            return RedirectToAction("Index", "InitialCo");
        }   
    }

    [HttpGet]
    public ActionResult Register()
    {
        return View(new Customer());
    }
    [HttpPost]
    public ActionResult Register(Customer customer)
    {
        try
        {
            repository.Register(customer);
            return RedirectToAction("Login");
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
            ViewBag.ErrorMessage = ex.Message;
            return View(customer);
        }
        
    }
}