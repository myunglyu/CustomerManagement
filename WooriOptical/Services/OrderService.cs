using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WooriOptical.Models;

namespace WooriOptical.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrderViewModel>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Select(o => new OrderViewModel
            {
                OrderId = o.OrderId,
                CustomerId = o.CustomerId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                PayoffStatus = o.PayoffStatus
            })
            .ToListAsync();
    }

    public async Task<OrderViewModel?> GetOrderByIdAsync(Guid orderId)
    {
        var order = await _context.Orders
            .Where(o => o.OrderId == orderId)
            .FirstOrDefaultAsync();

        if (order == null) return null;
        else
        {
            var model = new OrderViewModel
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                PrescriptionId = order.PrescriptionId,
                Height = order.Height,
                Frame = order.Frame,
                FramePrice = order.FramePrice,
                Lens = order.Lens,
                LensPrice = order.LensPrice,
                TotalAmount = order.TotalAmount,
                Discount = order.Discount,
                FinalAmount = order.FinalAmount,
                Deposit = order.Deposit,
                Balance = order.Balance,
                BalancePaid = order.BalancePaid,
                PayoffStatus = order.PayoffStatus
            };

            return model;
        }
    }
}