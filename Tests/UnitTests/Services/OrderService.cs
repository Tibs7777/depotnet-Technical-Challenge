﻿using Moq;
using Xunit;
using Repository.Contracts;
using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;
using Services;
using Entities.Exceptions;

//A quick word on testing since it was mentioned so much:

//I've only included unit tests in this challenge. I think Integration tests might be outside the scope of it (especially since unit tests were mentioned by name)
//I'd also rather look to a more robust process for integration testing than simply using in-memory databases due to their limitations, which is what I would do here if needed.
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
        public async Task GetOrdersAsync_ValidPageParameters_ReturnsListOfOrders()
        {
            //Arrange
            const int pageSize = 10;
            const int pageNumber = 1;

            var orderList = new List<Order> { new Order(), new Order() };
            _mockRepositoryManager.Setup(x => x.OrderRepository.GetOrdersAsync(pageSize, pageNumber)).ReturnsAsync(orderList);

            var orderDtoList = new List<OrderDto> { new OrderDto(), new OrderDto() };
            _mockMapper.Setup(x => x.Map<List<OrderDto>>(orderList)).Returns(orderDtoList);

            // Act
            List<OrderDto> result = await _service.GetOrdersAsync(pageSize, pageNumber);

            // Assert
            Assert.Equal(orderDtoList, result);
            Assert.IsType<List<OrderDto>>(result);
            _mockRepositoryManager.Verify(x => x.OrderRepository.GetOrdersAsync(pageSize, pageNumber), Times.Once);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(101, 1)]
        [InlineData(1, 0)]
        public async Task GetOrdersAsync_ThrowsBadRequestException_WhenInvalidPageSizeOrPageNumber(int pageSize, int pageNumber)
        {
            // Act and Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.GetOrdersAsync(pageSize, pageNumber));

            _mockRepositoryManager.Verify(x => x.OrderRepository.GetOrdersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _mockMapper.Verify(x => x.Map<List<OrderDto>>(It.IsAny<List<Order>>()), Times.Never);
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
            OrderDto result = await _service.GetOrderByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OrderDto>(result);
            Assert.Equal(orderDto, result);
            _mockRepositoryManager.Verify(x => x.OrderRepository.GetOrderByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetOrderAndCheckIfExists_ThrowsNotFoundException()
        {
            // Arrange
            const int orderId = 1;
            const Order? notFoundOrder = null;
            _mockRepositoryManager.Setup(x => x.OrderRepository.GetOrderByIdAsync(orderId)).ReturnsAsync(notFoundOrder);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetOrderByIdAsync(orderId));
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
            OrderDto result = await _service.CreateOrderAsync(orderForCreationDto);

            // Assert
            Assert.Equal(orderDto, result);
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
            List<OrderDetailDto> result = await _service.CreateOrderDetailsByOrderIdAsync(orderId, orderDetailsForCreation);

            // Assert
            Assert.Equal(finalOrderDetailDtos, result);
            _mockRepositoryManager.Verify(repoManager => repoManager.OrderRepository.GetOrderByIdAsync(orderId), Times.Once);
            _mockRepositoryManager.Verify(repoManager => repoManager.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderByIdAsync_ValidOrderId_DeletesOrder()
        {
            // Arrange
            const int orderId = 1;
            var orderToDelete = new Order { OrderId = orderId };

            _mockRepositoryManager.Setup(repoManager => repoManager.OrderRepository.GetOrderByIdAsync(orderId)).ReturnsAsync(orderToDelete);
            _mockRepositoryManager.Setup(repoManager => repoManager.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteOrderByIdAsync(orderId);

            // Assert
            _mockRepositoryManager.Verify(repoManager => repoManager.OrderRepository.GetOrderByIdAsync(orderId), Times.Once);
            _mockRepositoryManager.Verify(repoManager => repoManager.OrderRepository.DeleteOrder(orderToDelete), Times.Once);
            _mockRepositoryManager.Verify(repoManager => repoManager.SaveAsync(), Times.Once);
        }
    }
}