using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WooriOptical.Models;
using WooriOptical.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace WooriOptical.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ICustomerService _customerService;
    private readonly ILogger<OrderController> _logger;
    private readonly AppDbContext _context;

    public OrderController(IOrderService orderService, ICustomerService customerService, ILogger<OrderController> logger, AppDbContext context)
    {
        _orderService = orderService;
        _logger = logger;
        _context = context;
        _customerService = customerService;
    }

    // Order Actions
    public async Task<IActionResult> Index()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        foreach (var order in orders)
        {
            if (order.CustomerId != Guid.Empty)
            {
                var customer = await _customerService.GetCustomerByIdAsync(order.CustomerId);
                if (customer != null && customer.Name != null)
                {
                    order.CustomerName = customer.Name;
                    order.CustomerPhone = customer.Phone;
                }
                else
                {
                    order.CustomerName = "Unknown";
                    order.CustomerPhone = "Unknown";
                }
            }
        }
        return View(orders);
    }

    public async Task<IActionResult> Detail(Guid id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

}