using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WooriOptical.Models;
using WooriOptical.Services;
using Microsoft.AspNetCore.Authorization;

namespace WooriOptical.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICustomerService _customerService;

    public HomeController(ILogger<HomeController> logger, ICustomerService customerService)
    {
        _logger = logger;
        _customerService = customerService;
    }

    public async Task<IActionResult> Index(string? searchName, string? searchPhone)
    {
        IEnumerable<CustomerViewModel> customers;
        if (!string.IsNullOrWhiteSpace(searchName))
        {
            customers = await _customerService.FindCustomersByNameAsync(searchName);
        }
        else if (!string.IsNullOrWhiteSpace(searchPhone))
        {
            customers = await _customerService.FindCustomersByPhoneAsync(searchPhone);
        }
        else
        {
            customers = new List<CustomerViewModel>(); // Return empty list by default
        }
        ViewBag.SearchName = searchName;
        ViewBag.SearchPhone = searchPhone;
        return View(customers);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
