using GameHive.Models;
using GameHive.Models.enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.DataAccess.Repository.Repositories
{

    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Game)
                .FirstOrDefaultAsync(o => o.Id.Equals(orderId)) ?? throw new InvalidOperationException("Order not found");
        }

        public async Task<decimal> GetTotalPriceAsync(Guid orderId)
        {
            var total = await _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .SumAsync(od => od.Game.Price);

            return total;
        }


        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Game)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Game)
                .ToListAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task AddOrderDetailsAsync(List<OrderDetail> orderDetails)
        {
            _context.OrderDetails.AddRange(orderDetails);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(Guid orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> UpdateOrderDetailApprovalStatusAsync(Guid orderId, int gameId, PublisherApprovalStatusEnums status)
        {
            var orderDetail = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.GameId == gameId);

            if (orderDetail == null)
                return false;

            orderDetail.ApprovalStatus = status;
            orderDetail.ApprovalDate = DateTime.Now;

            // Update the order status if necessary
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order != null)
            {
                // Check if all order details are approved or rejected
                bool allProcessed = order.OrderDetails.All(od =>
                    od.ApprovalStatus == PublisherApprovalStatusEnums.Approved ||
                    od.ApprovalStatus == PublisherApprovalStatusEnums.Rejected);

                bool allApproved = order.OrderDetails.All(od =>
                    od.ApprovalStatus == PublisherApprovalStatusEnums.Approved);

                // Update order status based on order detail approval statuses
                if (allProcessed)
                {
                    order.Status = allApproved ? OrderStatusEnums.Processing : OrderStatusEnums.Rejected;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByPublisherAsync(string publisherId, PublisherApprovalStatusEnums? status = null)
        {
            var query = _context.OrderDetails
                .Include(od => od.Game)
                .Include(od => od.Order)
                .Where(od => od.Game.PublisherId == publisherId);

            if (status.HasValue)
            {
                query = query.Where(od => od.ApprovalStatus == status.Value);
            }

            return await query.OrderByDescending(od => od.Order.OrderDate).ToListAsync();
        }

        public async Task<int> GetOrderDetailsCountByStatusAsync(string publisherId, PublisherApprovalStatusEnums status)
        {
            return await _context.OrderDetails
                .Include(od => od.Game)
                .Where(od => od.Game.PublisherId == publisherId && od.ApprovalStatus == status)
                .CountAsync();
        }
    }

}
