using Microsoft.AspNetCore.Mvc;
using VLDocesWeb.Repositories;
using VLDocesWeb.Models;
namespace VLDocesWeb.Controllers;

public class OrderController : Controller
{
    private IProductRepository _productRepository;
    private IOrderRepository _orderRepository;
    public OrderController(IProductRepository productRepository, IOrderRepository orderRepository)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
    }

    public ActionResult Index()
    {
        var products = _productRepository.ListAll();
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

    public ActionResult Cart()
    {
        return View("Cart");
    }

    public ActionResult Payment()
    {
        return View("Payment");
    }

    public ActionResult Details(int id)
    {
        var idCliente = (int)HttpContext.Session.GetInt32("UserId");

        var order = _orderRepository.GetById(id);
        var items = _orderRepository.ListarItensPorPedido(id);

        // Passamos a Order como Model principal, e os Itens via ViewBag
        ViewBag.OrderItems = items;
        return View("Details", order);
    }

    public ActionResult History()
    {
        var idClienteSession = HttpContext.Session.GetInt32("UserId");

        if (!idClienteSession.HasValue || idClienteSession.Value <= 0)
        {
            // Se o ID não for encontrado ou for 0/negativo, redireciona.
            return RedirectToAction("Login", "Account"); 
        }

        int idCliente = idClienteSession.Value;

        var orders = _orderRepository.ListarPorCliente(idCliente);

        return View("History", orders);
    }
}