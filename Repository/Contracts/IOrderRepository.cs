using Entities.Models;

namespace Repository.Contracts
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersAsync(int pageSize, int pageNumber);
    }
}
