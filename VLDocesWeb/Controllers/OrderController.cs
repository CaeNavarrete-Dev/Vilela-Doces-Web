using Microsoft.AspNetCore.Mvc;
using VLDocesWeb.Repositories;
using VLDocesWeb.Models;
namespace VLDocesWeb.Controllers;
using System.Text.Json;
using System.Collections.Generic;
using VLDocesWeb.Models;
using Microsoft.VisualBasic;

public class OrderController : Controller
{
    private IProductRepository _productRepository;
    private IOrderRepository _orderRepository;
    public OrderController(IProductRepository productRepository, IOrderRepository orderRepository)
    {
        this._productRepository = productRepository;
        this._orderRepository = orderRepository;
    }

    public ActionResult Index(int categoriaId = 1)
    {
        HttpContext.Session.SetInt32("LastCategoryId", categoriaId);

        List<Product> products;
        if (categoriaId == 2)
        {
            products = _productRepository.ListAllPackage();
            ViewData["PageTitle"] = "Faça sua Encomenda";
            ViewBag.IsEncomenda = true;
        }
        else
        {
            products = _productRepository.ListAllOrder();
            ViewData["PageTitle"] = "Pronta Entrega";
            ViewBag.IsEncomenda = false;
        }

        ViewBag.CurrentCategory = categoriaId;
        return View(products);
    }

    public ActionResult ListOrder()
    {
        var orders = _orderRepository.Listar();
        return View(orders);
    }

    public ActionResult ListarPorStatus(int status)
    {
        var orders = _orderRepository.ListarPorStatus(status);
        return View("ListOrder", orders);
    }

    [HttpPost]
    public ActionResult AddToCart(int id)
    {
        List<CartItem> cart = GetCartFromSession();
        CartItem existeItem = null;
        
        // Variável para guardar a categoria e saber para onde voltar
        int categoriaDestino = 1; 

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
            categoriaDestino = existeItem.Produto.Id_Categoria; // Pega a categoria do item existente
        }
        else
        {
            Product produto = _productRepository.Read(id);
            if (produto != null)
            {
                cart.Add(new CartItem
                {
                    Produto = produto,
                    PrecoVendido = produto.Preco,
                    Quantidade = 1
                });
                categoriaDestino = produto.Id_Categoria; // Pega a categoria do novo produto
            }
        }
        
        SaveCartSession(cart);
        
        return RedirectToAction("Index", new { categoriaId = categoriaDestino });
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
    public ActionResult RemoveToCart(int id)
    {
        var cart = GetCartFromSession();
        CartItem existeItem = null;
        int categoriaDestino = 1;

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
            categoriaDestino = existeItem.Produto.Id_Categoria;

            existeItem.Quantidade -= 1;
            if (existeItem.Quantidade == 0)
            {
                cart.Remove(existeItem);
            }
        }
        else
        {
            var prod = _productRepository.Read(id);
            if(prod != null) categoriaDestino = prod.Id_Categoria;
        }

        SaveCartSession(cart);
        
        return RedirectToAction("Index", new { categoriaId = categoriaDestino });
    }

    [HttpGet]
    public ActionResult Cart()
    {
        List<CartItem> cart = GetCartFromSession();
        
        int lastCategory = HttpContext.Session.GetInt32("LastCategoryId") ?? 1;
        ViewBag.LastCategoryId = lastCategory;

        return View("Cart", cart);
    }

    public ActionResult Payment()
    {
        var summaryString = HttpContext.Session.GetString("PaymentSummary");
        if (string.IsNullOrEmpty(summaryString))
        {
           return RedirectToAction("Cart"); 
        } 

        var summary = JsonSerializer.Deserialize<PaymentSummaryViewModel>(summaryString);
        
        var cart = GetCartFromSession();
        // Consideramos encomenda se houver qualquer produto da categoria 2
        bool temEncomenda = cart.Any(c => c.Produto.Id_Categoria == 2);
        ViewBag.TemEncomenda = temEncomenda;

        return View("Payment", summary);
    }

    public ActionResult DetailsVini(int id)
    {
        var idCliente = (int)HttpContext.Session.GetInt32("UserId");

        var order = _orderRepository.GetById(id);
        var items = _orderRepository.ListarItensPorPedido(id);

        ViewBag.OrderItems = items;
        return View("DetailsVini", order);
    }

    public ActionResult History()
    {
        var idClienteSession = HttpContext.Session.GetInt32("UserId");

        if (!idClienteSession.HasValue || idClienteSession.Value <= 0)
        {
            return RedirectToAction("Login", "Account"); 
        }

        int idCliente = idClienteSession.Value;

        var orders = _orderRepository.ListarPorCliente(idCliente);

        return View("History", orders);
    }

    [HttpGet]
    public IActionResult CalcularFreteETotal(int enderecoId)
    {
        List<CartItem> cartItems = GetCartFromSession();

        decimal subtotal = cartItems.Sum(item => item.PrecoVendido * item.Quantidade);
        
        decimal frete;
        if (enderecoId == 1)
        {
            frete = 0;
        }
        else
        {
            frete = 5; 
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
        // 1. Recuperar carrinho para saber se é Encomenda ANTES de validar
        var cart = GetCartFromSession();
        bool temEncomenda = cart.Any(c => c.Produto.Id_Categoria == 2);

        // 2. Validação: Pronta Entrega (Dinheiro)
        // Só valida troco aqui se NÃO for encomenda e a forma for dinheiro (0)
        if (!temEncomenda && model.FormaPagamento == 0)
        {
            if (model.NaoPrecisoTroco)
            {
                model.ValorTroco = null; 
            } 
            else if (model.ValorTroco == null || model.ValorTroco <= model.TotalGeral)
            {
                ModelState.AddModelError("ValorTroco", "Para troco, o valor deve ser maior que o Total a Pagar.");
            }
        }

        // 3. Validação: Encomenda
        if (temEncomenda)
        {
            if (model.DataEntregaAgendada == null)
                ModelState.AddModelError("DataEntregaAgendada", "A Data de entrega é obrigatória para encomendas.");
            
            // Validação do troco caso escolha opção 3 (Sinal Pix + Restante Dinheiro)
            if (model.OpcaoPagamentoEncomenda == 3 && !model.NaoPrecisoTroco)
            {
                var metade = model.TotalGeral / 2;
                if (model.ValorTroco.HasValue && model.ValorTroco < metade)
                {
                    ModelState.AddModelError("ValorTroco", $"O valor para troco deve ser maior que o valor a pagar na entrega ({metade:C2})");
                }
            }
        }

        if (!ModelState.IsValid)
        {
            ViewBag.TemEncomenda = temEncomenda; 
            return View("Payment", GetPaymentSummary());
        }

        var summary = GetPaymentSummary();
        var customerId = HttpContext.Session.GetInt32("UserId"); 
        var enderecoId = HttpContext.Session.GetInt32("AddressId");

        if (customerId == null || enderecoId == null || !cart.Any())
        {
            return RedirectToAction("Cart");
        }

        int novoPedidoId = _orderRepository.Criar(
            cart, 
            summary, 
            model, 
            customerId.Value, 
            enderecoId.Value
        );

        if (novoPedidoId > 0)
        {
            HttpContext.Session.Remove("Cart");
            HttpContext.Session.Remove("PaymentSummary");
            HttpContext.Session.Remove("AddressId"); 
            
            return RedirectToAction("Index", "Initial");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Houve um erro ao processar seu pedido. Tente novamente.");
            ViewBag.TemEncomenda = temEncomenda;
            return View("Payment", summary);
        }
    }

    private PaymentSummaryViewModel GetPaymentSummary()
    {
        string summaryJson = HttpContext.Session.GetString("PaymentSummary");
        if (string.IsNullOrEmpty(summaryJson))
        {
          return new PaymentSummaryViewModel();  
        } 
        return JsonSerializer.Deserialize<PaymentSummaryViewModel>(summaryJson);
    }

    [HttpGet]
    public ActionResult Details(int id)
    {
        var orderDetails = _orderRepository.GetOrderDetails(id);
        if (orderDetails == null)
        {
            return RedirectToAction("Index");
        }
        return View("Details", orderDetails);
    }

    [HttpPost]
    public ActionResult UpdateOrderStatus(int orderId, int newStatus)
    {
        _orderRepository.UpdateOrderStatus(orderId, newStatus);
        return RedirectToAction("Details", new { id = orderId });
    }

    [HttpPost]
    public ActionResult UpdatePaymentStatus(int orderId, int newStatus)
    {
        _orderRepository.UpdatePaymentStatus(orderId, newStatus);
        return RedirectToAction("Details", new { id = orderId });
    }

    [HttpPost]
    public ActionResult AssignToMe(int orderId)
    {
        var collaboratorId = HttpContext.Session.GetInt32("UserId"); // (Confirme se a chave é "UserId")
        _orderRepository.AssignCollaborator(orderId, collaboratorId.Value);
        return RedirectToAction("Details", new { id = orderId });
    }

    [HttpPost]
    public ActionResult UpdateDeliveryStatus(int orderId, int newStatus)
    {
        _orderRepository.UpdateDeliveryStatus(orderId, newStatus);
        return RedirectToAction("Details", new { id = orderId });
    }
    [HttpGet]
    public IActionResult Cancel(int id)
    {
        var order = _orderRepository.GetById(id);
        if(order == null)
        {
            TempData["Message"] = "Pedido não encontrado.";
        }
        else if (order.Status == 0)
        {
            try
            {
                _orderRepository.CancelOrder(id);
                TempData["SuccessMessage"] = $"O Pedido número {id} foi cancelado com sucesso.";
            }
            catch
            {
                TempData["ErrorMessage"] = $"Erro ao cancelar o Pedido número {id}.";
            }
        }
        else
        {
            TempData["Message"] = $"O Pedido número {id} não pode ser cancelado pois já foi confirmado.";
        }
        return RedirectToAction("DetailsVini", new{id = id});
    }
}