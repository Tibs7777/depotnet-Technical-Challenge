using Entities.Models;

namespace Repository.Contracts
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersAsync(int pageSize, int pageNumber);

        Task<Order?> GetOrderByIdAsync(int orderId);

        void CreateOrder(Order order);
    }
}
