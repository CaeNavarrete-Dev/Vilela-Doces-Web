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
    private IOrderRepository _orderRepository;
    public OrderController(IProductRepository repository, IOrderRepository orderRepository)
    {
        this.repository = repository;
        this._orderRepository = orderRepository;
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
        // 1. ========= VALIDAÇÃO DO FORMULÁRIO (Seu código está ótimo) ==========
        
        // Validação de Troco (um pouco mais robusta)
        if (model.FormaPagamento == 0) // 0 = Dinheiro
        {
            if (model.NaoPrecisoTroco)
            {
                model.ValorTroco = null; // Limpa o valor se a caixa estiver marcada
            }
            // Se "NaoPrecisoTroco" NÃO está marcada E o valor é inválido...
            else if (!model.NaoPrecisoTroco && (model.ValorTroco == null || model.ValorTroco <= model.TotalGeral))
            {
                ModelState.AddModelError("ValorTroco", "Para troco, o valor deve ser maior que o Total a Pagar.");
            }
        }
        
        // Se o modelo geral for inválido (incluindo o erro de troco acima)
        if (!ModelState.IsValid)
        {
            // Retorna para a view de Pagamento, enviando o resumo de volta
            return View("Payment", GetPaymentSummary());
        }

        // 2. ========= COLETA DE DADOS DA SESSÃO ==========
        
        var cart = GetCartFromSession();
        var summary = GetPaymentSummary();

        // Baseado no seu AddressController e no login:
        var customerId = HttpContext.Session.GetInt32("UserId"); 
        var enderecoId = HttpContext.Session.GetInt32("AddressId");

        // 3. ========= VERIFICAÇÃO DE SEGURANÇA ==========
        // Se a sessão expirou ou o carrinho está vazio
        if (customerId == null || enderecoId == null || !cart.Any())
        {
            // Se faltar dados vitais, mande o usuário de volta para o início.
            return RedirectToAction("Cart");
        }

        // 4. ========= CHAMADA DO REPOSITÓRIO (A MÁGICA) ==========
        
        // Chamamos o método CreateOrder com os 5 argumentos que ele espera
        int novoPedidoId = _orderRepository.CreateOrder(
            cart, 
            summary, 
            model, // 'model' tem FormaPagamento, Observacoes, etc.
            customerId.Value, 
            enderecoId.Value // Passando o ID do endereço vindo da Sessão
        );

        // 5. ========= RESPOSTA (Sucesso ou Falha) ==========
        if (novoPedidoId > 0)
        {
            // SUCESSO!
            // Limpe tudo da sessão para o próximo pedido
            HttpContext.Session.Remove("Cart");
            HttpContext.Session.Remove("PaymentSummary");
            HttpContext.Session.Remove("AddressId"); // Limpe o endereço também
            
            // Redirecione para a página de Detalhes do Pedido
            return RedirectToAction("Index","Initial");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Houve um erro ao processar seu pedido. Tente novamente.");
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
}