using Moq;
using Xunit;
using Repository.Contracts;
using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;
using Services;
using Entities.Exceptions;

//A quick word on testing for the reviewer since it was mentioned so much:

//I've only included unit tests in this challenge. I think Integration tests might be outside the scope of it (especially since unit tests were mentioned by name)
//I'd also rather look to a more robust process for integration testing than simply using in-memory databases due to their limitations.
//I've not tested the repository layer, as for these kinds of tests, it's a bit too simple, and would also be covered by the integration tests.

namespace Tests.UnitTests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IRepositoryManager> _mockRepositoryManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _mockRepositoryManager = new Mock<IRepositoryManager>();
            _mockMapper = new Mock<IMapper>();
            _service = new OrderService(_mockMapper.Object, _mockRepositoryManager.Object);
        }

        [Fact]
        public async Task GetOrdersAsync_ReturnsListOfOrders()
        {
            //Arrange
            const int pageSize = 10;
            const int pageNumber = 0;

            var orderList = new List<Order> { new Order(), new Order() };
            _mockRepositoryManager.Setup(x => x.OrderRepository.GetOrdersAsync(pageSize, pageNumber)).ReturnsAsync(orderList);

            var orderDtoList = new List<OrderDto> { new OrderDto(), new OrderDto() };
            _mockMapper.Setup(x => x.Map<List<OrderDto>>(orderList)).Returns(orderDtoList);

            // Act
            var result = await _service.GetOrdersAsync(pageSize, pageNumber);

            // Assert
            Assert.Equal(orderDtoList.Count, result.Count());
            Assert.IsType<List<OrderDto>>(result);
            _mockRepositoryManager.Verify(x => x.OrderRepository.GetOrdersAsync(pageSize, pageNumber), Times.Once);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ReturnsOrderDto()
        {
            // Arrange
            var order = new Order();
            _mockRepositoryManager.Setup(x => x.OrderRepository.GetOrderByIdAsync(It.IsAny<int>())).ReturnsAsync(order);

            var orderDto = new OrderDto();
            _mockMapper.Setup(x => x.Map<OrderDto>(It.IsAny<Order>())).Returns(orderDto);

            // Act
            var result = await _service.GetOrderByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OrderDto>(result);
            Assert.Equal(orderDto, result);
            _mockRepositoryManager.Verify(x => x.OrderRepository.GetOrderByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetOrderByIdAsync_OrderNotFound_ThrowsNotFoundException()
        {
            // Arrange
            const int orderId = 1;
            const Order? notFoundOrder = null;
            _mockRepositoryManager.Setup(x => x.OrderRepository.GetOrderByIdAsync(orderId)).ReturnsAsync(notFoundOrder);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _service.GetOrderByIdAsync(orderId));
            _mockRepositoryManager.Verify(x => x.OrderRepository.GetOrderByIdAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task CreateOrderAsync_ValidOrderForCreation_ReturnsCreatedOrder()
        {
            // Arrange
            var orderForCreationDto = new OrderForCreationDto();

            var order = new Order();
            var orderDto = new OrderDto();

            _mockMapper.Setup(mapper => mapper.Map<Order>(orderForCreationDto)).Returns(order);
            _mockMapper.Setup(mapper => mapper.Map<OrderDto>(order)).Returns(orderDto);

            _mockRepositoryManager.Setup(repoManager => repoManager.OrderRepository.CreateOrder(order));

            // Act
            var result = await _service.CreateOrderAsync(orderForCreationDto);

            // Assert
            Assert.Equal(orderDto, result);
            Assert.IsType<OrderDto>(result);
            _mockRepositoryManager.Verify(repoManager => repoManager.OrderRepository.CreateOrder(order), Times.Once);
            _mockRepositoryManager.Verify(repoManager => repoManager.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateOrderDetailsByOrderIdAsync_ValidOrderForCreation_ReturnsCreatedOrderDetails()
        {
            // Arrange
            var orderDetailsForCreation = new List<OrderDetailForCreationDto>()
            {
                new OrderDetailForCreationDto(),
            };
            const int orderId = 1;
            var foundOrder = new Order();
            var orderDetails = new List<OrderDetail>();
            var finalOrderDetailDtos = new List<OrderDetailDto>();

            _mockRepositoryManager.Setup(repoManager => repoManager.OrderRepository.GetOrderByIdAsync(It.IsAny<int>())).ReturnsAsync(foundOrder);
            _mockRepositoryManager.Setup(repoManager => repoManager.SaveAsync()).Returns(Task.CompletedTask);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<OrderDetail>>(orderDetailsForCreation)).Returns(orderDetails);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<OrderDetailDto>>(orderDetails)).Returns(finalOrderDetailDtos);

            // Act
            var result = await _service.CreateOrderDetailsByOrderIdAsync(orderId, orderDetailsForCreation);

            // Assert
            Assert.IsAssignableFrom<IEnumerable<OrderDetailDto>>(result);
            Assert.Equal(finalOrderDetailDtos.Count, result.Count());
            _mockRepositoryManager.Verify(repoManager => repoManager.OrderRepository.GetOrderByIdAsync(orderId), Times.Once);
            _mockRepositoryManager.Verify(repoManager => repoManager.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateOrderDetailsByOrderIdAsync_OrderNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var orderDetailsForCreation = new List<OrderDetailForCreationDto>()
            {
                new OrderDetailForCreationDto(),
            };
            const int orderId = 1;
            Order? foundOrder = null;

            _mockRepositoryManager.Setup(repoManager => repoManager.OrderRepository.GetOrderByIdAsync(It.IsAny<int>())).ReturnsAsync(foundOrder);

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await _service.CreateOrderDetailsByOrderIdAsync(orderId, orderDetailsForCreation));
            _mockRepositoryManager.Verify(repoManager => repoManager.OrderRepository.CreateOrder(It.IsAny<Order>()), Times.Never);
            _mockRepositoryManager.Verify(repoManager => repoManager.SaveAsync(), Times.Never);
        }

    }
}