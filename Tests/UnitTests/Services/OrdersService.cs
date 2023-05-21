﻿using Moq;
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
            Assert.Equal(orderDtoList.Count, result.Count);
            Assert.IsType<List<OrderDto>>(result);
            _mockRepositoryManager.Verify(x => x.OrderRepository.GetOrdersAsync(pageSize, pageNumber), Times.Once);
            _mockMapper.Verify(x => x.Map<List<OrderDto>>(orderList), Times.Once);
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
            _mockMapper.Verify(x => x.Map<OrderDto>(order), Times.Once);
        }

        [Fact]
        public async Task GetOrderByIdAsync_OrderNotFound_ThrowsNotFoundException()
        {
            // Arrange
            const int orderId = 1;
            _mockRepositoryManager.Setup(x => x.OrderRepository.GetOrderByIdAsync(orderId)).ReturnsAsync((Order)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _service.GetOrderByIdAsync(orderId));
            _mockRepositoryManager.Verify(x => x.OrderRepository.GetOrderByIdAsync(orderId), Times.Once);
        }

    }
}