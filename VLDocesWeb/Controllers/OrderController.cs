using Microsoft.AspNetCore.Mvc;
using VLDocesWeb.Repositories;
namespace VLDocesWeb.Controllers;
using System.Text.Json;
using System.Collections.Generic;
using VLDocesWeb.Models;
using Microsoft.VisualBasic;

public class OrderController : Controller
{
    private IProductRepository repository;
    public OrderController(IProductRepository repository)
    {
        this.repository = repository;
    }
    public ActionResult Index()
    {
        var _products = repository.ListAllOrder();
        return View(_products);
    }

    [HttpPost]
    public ActionResult AddToCart(int id)
    {
        List<CartItem> cart = GetCartFromSession();
        CartItem existeItem = null;
        foreach (CartItem item in cart)
        {
            if (item.Produto.Id_Produto == id)
            {
                existeItem = item;
                break;
            }
        }
        if (existeItem != null)
        {
            existeItem.Quantidade += 1;
        }
        else
        {
            Product produto = repository.Read(id);
            if (produto != null)
            {
                cart.Add(new CartItem
                {
                    Produto = produto,
                    Quantidade = 1
                });
            }
        }
        SaveCartSession(cart);
        return RedirectToAction("Index");
    }
    private List<CartItem> GetCartFromSession()
    {
        string cartJson = HttpContext.Session.GetString("Cart");

        if (string.IsNullOrEmpty(cartJson))
        {
            return new List<CartItem>();
        }

        return JsonSerializer.Deserialize<List<CartItem>>(cartJson); //Traduz a string para uma lista
    }
    private void SaveCartSession(List<CartItem> cart)
    {
        string cartJson = JsonSerializer.Serialize(cart);

        HttpContext.Session.SetString("Cart", cartJson);
    }
    [HttpPost]
    public ActionResult RemoveToCart (int id)
    {
        var cart = GetCartFromSession();
        CartItem existeItem = null;

        foreach (CartItem item in cart)
        {
            if (item.Produto.Id_Produto == id)
            {
                existeItem = item;
            }
        }
        if (existeItem != null)
        {
            existeItem.Quantidade -= 1;
            if (existeItem.Quantidade == 0)
            {
                cart.Remove(existeItem);
            }
        }
        SaveCartSession(cart);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Cart()
    {
        List<CartItem> cart = GetCartFromSession();
        return View("Cart", cart);
    }

    public ActionResult Payment()
    {
        return View("Payment");
    }

    public ActionResult Details()
    {
        return View("Details");
    }

    public ActionResult History()
    {
        return View("History");
    }
}