using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WooriOptical.Models;
using WooriOptical.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace WooriOptical.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ICustomerService _customerService;
    private readonly IPrescriptionService _prescriptionService;
    private readonly ILogger<OrderController> _logger;
    private readonly AppDbContext _context;

    public OrderController(IOrderService orderService, ICustomerService customerService, IPrescriptionService prescriptionService, ILogger<OrderController> logger, AppDbContext context)
    {
        _orderService = orderService;
        _logger = logger;
        _context = context;
        _customerService = customerService;
        _prescriptionService = prescriptionService;
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

        var customer = await _customerService.GetCustomerByIdAsync(order.CustomerId);
        if (customer == null)
        {
            customer = new CustomerViewModel { Name = "Unknown", Phone = "Unknown" };
        }

        var model = new OrderViewModel
        {
            OrderId = order.OrderId,
            PrescriptionId = order.PrescriptionId,
            CustomerName = customer.Name,
            CustomerPhone = customer.Phone,
            OrderDate = order.OrderDate,
            Frame = order.Frame,
            Lens = order.Lens,
            TotalAmount = order.TotalAmount,
            Balance = order.Balance,
            PayoffStatus = order.PayoffStatus
        };

        ViewBag.Prescription = await _prescriptionService.GetPrescriptionByIdAsync(order.PrescriptionId);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        var model = new CustomerViewModel[customers.Count()];

        int i = 0;
        foreach (var customer in customers)
        {
            model[i] = new CustomerViewModel
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Phone = customer.Phone
            };
            i++;
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> SelectPrescription(Guid customerId)
    {
        var customer = await _customerService.GetCustomerByIdAsync(customerId);
        var prescriptions = await _prescriptionService.GetPrescriptionByCustomerIdAsync(customerId);

        if (customer == null)
        {
            return NotFound();
        }

        var model = new CustomerViewModel
        {
            CustomerId = customer.CustomerId,
            Name = customer.Name,
            Phone = customer.Phone,
            Prescriptions = prescriptions.Where(p => p != null).Cast<Prescription>().ToList()
        };

        return View(model);
    }
    
}