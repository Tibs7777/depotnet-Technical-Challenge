using AutoMapper;
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
        public async Task<List<OrderDto>> GetOrdersAsync(int pageSize, int pageNumber)
        {
            var orders = await _repositoryManager.OrderRepository.GetOrdersAsync(pageSize, pageNumber);

            return _mapper.Map<List<OrderDto>>(orders);
        }
    }
}
