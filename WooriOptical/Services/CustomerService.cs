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
                Address = c.Address
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
                Address = c.Address
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
                Address = c.Address
            })
            .ToListAsync();
    }

    public async Task<CustomerViewModel?> GetCustomerByIdAsync(Guid customerId)
    {
        return await _context.Customers
            .Where(c => c.CustomerId == customerId)
            .Select(c => new CustomerViewModel
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                Address = c.Address
            })
            .FirstOrDefaultAsync();
    }

    public async Task AddCustomerAsync(CustomerViewModel customer)
    {
        var entity = new Customer
        {
            CustomerId = customer.CustomerId == Guid.Empty ? Guid.NewGuid() : customer.CustomerId,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address
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
            entity.Address = customer.Address;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteCustomerAsync(Guid customerId)
    {
        var entity = await _context.Customers.FindAsync(customerId);
        if (entity != null)
        {
            _context.Customers.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

}