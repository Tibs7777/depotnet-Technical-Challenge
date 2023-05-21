using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<List<Order>> GetOrdersAsync(int pageSize, int pageNumber)
        {
            return await RepositoryContext.Orders.Include(o => o.OrderDetails).Skip(pageNumber * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await RepositoryContext.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public void CreateOrder(Order order) => Create(order);
    }
}
