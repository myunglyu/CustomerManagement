using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WooriOptical.Models;

namespace WooriOptical.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync()
    {
        return await _context.Customers
            .Select(c => new CustomerViewModel
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                Street = c.Street,
                City = c.City,
                State = c.State,
                Zip = c.Zip
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<CustomerViewModel>> FindCustomersByPhoneAsync(string phone)
    {
        return await _context.Customers
            .Where(c => c.Phone != null && c.Phone.Contains(phone))
            .Select(c => new CustomerViewModel
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                Street = c.Street,
                City = c.City,
                State = c.State,
                Zip = c.Zip
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<CustomerViewModel>> FindCustomersByNameAsync(string name)
    {
        return await _context.Customers
            .Where(c => c.Name != null && EF.Functions.Like(c.Name.ToLower(), $"%{name.ToLower()}%"))
            .Select(c => new CustomerViewModel
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                Street = c.Street,
                City = c.City,
                State = c.State,
                Zip = c.Zip
            })
            .ToListAsync();
    }

    public async Task<CustomerViewModel?> GetCustomerByIdAsync(Guid customerId)
    {
        var customer = await _context.Customers
            .Where(c => c.CustomerId == customerId)
            .FirstOrDefaultAsync();

        if (customer == null)
            return null;

        var prescriptions = await _context.Prescriptions
            .Where(p => p.CustomerId == customerId)
            .ToListAsync();

        var orders = await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .ToListAsync();

        return new CustomerViewModel
        {
            CustomerId = customer.CustomerId,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            Street = customer.Street,
            City = customer.City,
            State = customer.State,
            Zip = customer.Zip,
            Prescriptions = prescriptions,
            Orders = orders
        };
    }

    public async Task AddCustomerAsync(CustomerViewModel customer)
    {
        var entity = new Customer
        {
            CustomerId = customer.CustomerId == Guid.Empty ? Guid.NewGuid() : customer.CustomerId,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
                Street = customer.Street,
                City = customer.City,
                State = customer.State,
                Zip = customer.Zip
        };
        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCustomerAsync(CustomerViewModel customer)
    {
        var entity = await _context.Customers.FindAsync(customer.CustomerId);
        if (entity != null)
        {
            entity.Name = customer.Name;
            entity.Email = customer.Email;
            entity.Phone = customer.Phone;
                entity.Street = customer.Street;
                entity.City = customer.City;
                entity.State = customer.State;
                entity.Zip = customer.Zip;
            await _context.SaveChangesAsync();
        }
    }

    // public async Task DeleteCustomerAsync(Guid customerId)
    // {
    //     var entity = await _context.Customers.FindAsync(customerId);
    //     if (entity != null)
    //     {
    //         _context.Customers.Remove(entity);
    //         await _context.SaveChangesAsync();
    //     }
    // }

}