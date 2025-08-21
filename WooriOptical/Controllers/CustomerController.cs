using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WooriOptical.Models;
using WooriOptical.Services;
using Microsoft.Extensions.Configuration;

namespace WooriOptical.Controllers;

public class CustomerController : Controller
{
    private readonly ICustomerService _customerService;
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public CustomerController(ICustomerService customerService, AppDbContext context, IConfiguration configuration)
    {
        _customerService = customerService;
        _context = context;
        _configuration = configuration;
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
    ViewBag.GooglePlacesApiKey = _configuration["GooglePlaces:ApiKey"];
    return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CustomerViewModel model)
    {
        // Normalize phone to digits-only and enforce 10 digits
        if (!string.IsNullOrEmpty(model.Phone))
        {
            var digits = new string(model.Phone.Where(char.IsDigit).ToArray());
            if (digits.Length != 10)
            {
                ModelState.AddModelError(nameof(model.Phone), "Phone number must contain 10 digits.");
            }
            else
            {
                model.Phone = digits;
            }
        }

        if (ModelState.IsValid)
        {
            await _customerService.AddCustomerAsync(model);
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    // Address validation/lookup removed.

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
        // Normalize phone
        if (!string.IsNullOrEmpty(model.Phone))
        {
            var digits = new string(model.Phone.Where(char.IsDigit).ToArray());
            if (digits.Length != 10)
            {
                ModelState.AddModelError(nameof(model.Phone), "Phone number must contain 10 digits.");
            }
            else
            {
                model.Phone = digits;
            }
        }

        if (ModelState.IsValid)
        {
            await _customerService.UpdateCustomerAsync(model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // public async Task<IActionResult> Delete(Guid id)
    // {
    //     var customer = await _customerService.GetCustomerByIdAsync(id);
    //     if (customer == null)
    //         return NotFound();
    //     return View(customer);
    // }

    // [HttpPost, ActionName("Delete")]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> DeleteConfirmed(Guid id)
    // {
    //     await _customerService.DeleteCustomerAsync(id);
    //     return RedirectToAction(nameof(Index));
    // }

    // Prescription Actions
    public async Task<IActionResult> CreatePrescription(Guid customerId)
    {
        var customer = await _customerService.GetCustomerByIdAsync(customerId);
        if (customer == null)
            return NotFound();

        var prescription = new Prescription
        {
            PrescriptionId = Guid.NewGuid(),
            CustomerId = customerId,
            DateIssued = DateTime.Now
        };

        return View(prescription);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePrescription(Prescription model)
    {
        if (ModelState.IsValid)
        {
            model.PrescriptionId = Guid.NewGuid();
            _context.Prescriptions.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = model.CustomerId });
        }
        return View(model);
    }

    public async Task<IActionResult> Prescription(Guid id)
    {
        var prescription = await _context.Prescriptions.FindAsync(id);
        if (prescription == null)
            return NotFound();
        return View(prescription);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPrescription(Prescription model)
    {
        if (ModelState.IsValid)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = model.CustomerId });
        }
        return View("Prescription", model);
    }

    public async Task<IActionResult> DeletePrescription(Guid id)
    {
        var prescription = await _context.Prescriptions.FindAsync(id);
        if (prescription == null)
            return NotFound();

        _context.Prescriptions.Remove(prescription);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = prescription.CustomerId });
    }

    // Order Actions
    public async Task<IActionResult> CreateOrder(Guid customerId)
    {
        var customer = await _customerService.GetCustomerByIdAsync(customerId);
        if (customer == null)
            return NotFound();

        // Get customer's prescriptions for dropdown
        var prescriptions = await _context.Prescriptions
            .Where(p => p.CustomerId == customerId)
            .OrderByDescending(p => p.DateIssued)
            .ToListAsync();

        ViewBag.Prescriptions = prescriptions;

        var order = new Order
        {
            OrderId = Guid.NewGuid(),
            CustomerId = customerId,
            OrderDate = DateTime.Now,
            PayoffStatus = "Pending"
        };

        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOrder(Order model)
    {
        if (ModelState.IsValid)
        {
            model.OrderId = Guid.NewGuid();
            // Calculate balance server-side from FinalAmount and Deposit
            double finalAmt = 0;
            double depAmt = 0;
            if (!string.IsNullOrEmpty(model.FinalAmount) && double.TryParse(model.FinalAmount, out var f)) finalAmt = f;
            if (!string.IsNullOrEmpty(model.Deposit) && double.TryParse(model.Deposit, out var d)) depAmt = d;
            var computedBalance = finalAmt - depAmt;
            model.Balance = computedBalance.ToString("F2");

            // Calculate payment status based on deposit and computed balance
            model.PayoffStatus = CalculatePaymentStatus(model);

            _context.Orders.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = model.CustomerId });
        }
        return View(model);
    }

    public async Task<IActionResult> OrderDetails(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return NotFound();
        return View(order);
    }

    public async Task<IActionResult> EditOrder(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return NotFound();
        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditOrder(Order model)
    {
        if (ModelState.IsValid)
        {
            // Recompute balance server-side from FinalAmount and Deposit
            double finalAmt = 0;
            double depAmt = 0;
            if (!string.IsNullOrEmpty(model.FinalAmount) && double.TryParse(model.FinalAmount, out var f)) finalAmt = f;
            if (!string.IsNullOrEmpty(model.Deposit) && double.TryParse(model.Deposit, out var d)) depAmt = d;
            var computedBalance = finalAmt - depAmt;
            model.Balance = computedBalance.ToString("F2");

            // Calculate payment status based on deposit and computed balance
            model.PayoffStatus = CalculatePaymentStatus(model);

            _context.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(OrderDetails), new { id = model.OrderId });
        }
        return View(model);
    }

    private string CalculatePaymentStatus(Order order)
    {
        // Parse deposit and balance values
        double deposit = 0;
        double balance = 0;
        double balancePaid = 0;

        if (!string.IsNullOrEmpty(order.Deposit) && double.TryParse(order.Deposit, out double depositValue))
            deposit = depositValue;

        if (!string.IsNullOrEmpty(order.Balance) && double.TryParse(order.Balance, out double balanceValue))
            balance = balanceValue;

        if (!string.IsNullOrEmpty(order.BalancePaid) && double.TryParse(order.BalancePaid, out double balancePaidValue))
            balancePaid = balancePaidValue;

        // Payment status logic:
        // if deposit = 0 then "Pending"
        // if balance due = 0 or balance due = balance paid then "Paid"
        // else "Partial"
        
        if (deposit == 0)
            return "Pending";
        
        if (balance <= 0 || (balance > 0 && balance == balancePaid))
            return "Paid";
        
        return "Partial";
    }
}