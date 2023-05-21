using Shared.DataTransferObjects;

namespace Services.Contracts
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetOrdersAsync(int pageSize, int pageNumber);
    }
}
