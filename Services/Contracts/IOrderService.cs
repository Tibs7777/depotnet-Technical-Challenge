using Shared.DataTransferObjects;

namespace Services.Contracts
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetOrdersAsync(int pageSize, int pageNumber);
        Task<OrderDto> GetOrderByIdAsync(int orderId);
        Task<OrderDto> CreateOrderAsync(OrderForCreationDto order);
        Task DeleteOrderByIdAsync(int orderId);
        Task<List<OrderDetailDto>> CreateOrderDetailsByOrderIdAsync(int orderId, IEnumerable<OrderDetailForCreationDto> orderDetailForCreationDto);
    }
}
