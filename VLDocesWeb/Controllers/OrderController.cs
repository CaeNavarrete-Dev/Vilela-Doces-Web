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

    public ActionResult Details()
    {
        return View("Details");
    }

    public ActionResult History()
    {
        return View("History");
    }
}