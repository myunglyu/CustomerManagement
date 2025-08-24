using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WooriOptical.Models;

namespace WooriOptical.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderViewModel>> GetAllOrdersAsync();
    Task<OrderViewModel?> GetOrderByIdAsync(Guid orderId);
    // Task AddOrderAsync(OrderViewModel order);
    // Task UpdateOrderAsync(OrderViewModel order);
    // Task DeleteOrderAsync(Guid orderId);
}
