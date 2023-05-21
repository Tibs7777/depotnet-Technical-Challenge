using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using Repository.Contracts;
using Services.Contracts;
using Shared.DataTransferObjects;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;
        public OrderService(IMapper mapper, IRepositoryManager repositoryManager)
        {
            _mapper = mapper;
            _repositoryManager = repositoryManager;
        }
        public async Task<IEnumerable<OrderDto>> GetOrdersAsync(int pageSize, int pageNumber)
        {
            var orders = await _repositoryManager.OrderRepository.GetOrdersAsync(pageSize, pageNumber);

            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await GetOrderAndCheckIfExists(orderId);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> CreateOrderAsync(OrderForCreationDto orderForCreationDto)
        {
            var order = _mapper.Map<Order>(orderForCreationDto);

            _repositoryManager.OrderRepository.CreateOrder(order);

            await _repositoryManager.SaveAsync();

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDetailDto>> CreateOrderDetailsByOrderIdAsync(int orderId, IEnumerable<OrderDetailForCreationDto> orderDetailForCreationDto)
        {
            var order = await GetOrderAndCheckIfExists(orderId);

            var orderDetails = _mapper.Map<IEnumerable<OrderDetail>>(orderDetailForCreationDto);

            foreach(var orderDetail in orderDetails)
            {
                order.OrderDetails.Add(orderDetail);
            }

            await _repositoryManager.SaveAsync();

            return _mapper.Map<IEnumerable<OrderDetailDto>>(orderDetails);
        }

        public async Task DeleteOrderByIdAsync(int orderId)
        {
            var order = await GetOrderAndCheckIfExists(orderId);

            _repositoryManager.OrderRepository.DeleteOrder(order);

            await _repositoryManager.SaveAsync();
        }

        private async Task<Order> GetOrderAndCheckIfExists(int orderId)
        {
            var order = await _repositoryManager.OrderRepository.GetOrderByIdAsync(orderId);

            if (order == null)
                throw new NotFoundException($"Order with id {orderId} not found");

            return order;
        }
    }
}
