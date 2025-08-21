using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WooriOptical.Models;

namespace WooriOptical.Services;

public interface ICustomerService
{
    Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync();
    Task<CustomerViewModel?> GetCustomerByIdAsync(Guid customerId);
    Task<IEnumerable<CustomerViewModel>> FindCustomersByPhoneAsync(string phone);
    Task<IEnumerable<CustomerViewModel>> FindCustomersByNameAsync(string name);
    Task AddCustomerAsync(CustomerViewModel customer);
    Task UpdateCustomerAsync(CustomerViewModel customer);
    Task DeleteCustomerAsync(Guid customerId);

}
