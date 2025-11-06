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
                    PrecoVendido = produto.Preco,
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
        var summaryString = HttpContext.Session.GetString("PaymentSummary");
        var summary = JsonSerializer.Deserialize<PaymentSummaryViewModel>(summaryString);
        return View("Payment", summary);
    }

    public ActionResult Details()
    {
        return View("Details");
    }

    public ActionResult History()
    {
        return View("History");
    }

    [HttpGet]
    public IActionResult CalcularFreteETotal(int enderecoId)
    {
        List<CartItem> cartItems = GetCartFromSession();

        decimal subtotal = cartItems.Sum(item => item.PrecoVendido * item.Quantidade);
        
        decimal frete;
        if (enderecoId == 1)
        {
            frete = 0.00F;
        }
        else
        {
            frete = 5.00F; 
        }

        decimal totalGeral = subtotal + frete;

        var summary = new PaymentSummaryViewModel {
            Subtotal = subtotal,
            Frete = frete,
            TotalGeral = totalGeral
        };

        string summaryJson = JsonSerializer.Serialize(summary);

        HttpContext.Session.SetString("PaymentSummary", summaryJson);

        return Json(new { 
            subtotalFormatado = subtotal.ToString("C2", new System.Globalization.CultureInfo("pt-BR")),
            frete = frete.ToString("C2", new System.Globalization.CultureInfo("pt-BR")),
            totalGeral = totalGeral.ToString("C2", new System.Globalization.CultureInfo("pt-BR")),
            freteDecimal = frete
        });
    }

    [HttpPost]
    public IActionResult ProcessPayment(PaymentSubmissionViewModel model)
    {
        if (string.IsNullOrEmpty(model.FormaPagamento))
        {
            ModelState.AddModelError(string.Empty, "Por favor, selecione uma forma de pagamento.");
            return View("Payment", GetPaymentSummary());
        }

        if (model.FormaPagamento == "0")
        {
            if (model.NaoPrecisoTroco)
            {
                model.ValorTroco = null; 
            }
            else if (model.ValorTroco == null || model.ValorTroco <= model.TotalGeral)
            {
                ModelState.AddModelError("ValorTroco", "O valor de troco é obrigatório e precisa ser maior que o Total a Pagar.");
                return View("Payment", GetPaymentSummary());
            }
        }
        
        PaymentSummaryViewModel summary = HttpContext.Session.GetObjectFromJson<PaymentSummaryViewModel>("PaymentSummary");
        
        // Função para a finalização do pedido
        // ...
        // Lógica para Salvar o Pedido no Banco de Dados
        // ...

        if (model.FormaPagamento == "1")
        {
            // return RedirectToAction("PixConfirmation"); 
        }
        else
        {
            return RedirectToAction("Details"); 
        }
    }
}