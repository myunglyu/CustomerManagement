using Microsoft.AspNetCore.Mvc;
using WooriOptical.Models;
using WooriOptical.Services;

namespace WooriOptical.Controllers;

public class CustomerController : Controller
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
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
            customers = await _customerService.GetAllCustomersAsync();
        }
        ViewBag.SearchName = searchName;
        ViewBag.SearchPhone = searchPhone;
        return View(customers);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
            return NotFound();
        return View(customer);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CustomerViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _customerService.AddCustomerAsync(model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
            return NotFound();
        return View(customer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CustomerViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _customerService.UpdateCustomerAsync(model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
            return NotFound();
        return View(customer);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _customerService.DeleteCustomerAsync(id);
        return RedirectToAction(nameof(Index));
    }
}