using Shared.DataTransferObjects;

namespace Services.Contracts
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrdersAsync(int pageSize, int pageNumber);
        Task<OrderDto> GetOrderByIdAsync(int orderId);
        Task<OrderDto> CreateOrderAsync(OrderForCreationDto order);
        Task<IEnumerable<OrderDetailDto>> CreateOrderDetailsByOrderIdAsync(int orderId, IEnumerable<OrderDetailForCreationDto> orderDetailForCreationDto);
    }
}
