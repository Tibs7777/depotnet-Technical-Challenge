using Moq;
using Xunit;
using Repository.Contracts;
using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;
using Services;

//A quick word on testing for the reviewer since it was mentioned so much:

//I've only included unit tests in this challenge. I think Integration tests might be outside the scope of it (especially since unit tests were mentioned by name)
//I'd also rather look to a more robust process for integration testing than simply using in-memory databases due to their limitations.
//I've not tested the repository layer, as for these kinds of tests, it's a bit too simple, and would also be covered by the integration tests.

namespace Tests.UnitTests.Services
{
    public class OrderServiceTests
    {
        [Fact]
        public async Task GetOrdersAsync_ReturnsListOfOrders()
        {
            // Arrange
            var mockRepositoryManager = new Mock<IRepositoryManager>();
            var mockMapper = new Mock<IMapper>();
            var service = new OrderService(mockMapper.Object, mockRepositoryManager.Object);

            var orderList = new List<Order> { new Order(), new Order() };
            mockRepositoryManager.Setup(x => x.OrderRepository.GetOrdersAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(orderList);

            var orderDtoList = new List<OrderDto> { new OrderDto(), new OrderDto() };
            mockMapper.Setup(x => x.Map<List<OrderDto>>(It.IsAny<List<Order>>())).Returns(orderDtoList);

            // Act
            var result = await service.GetOrdersAsync(10, 0);

            // Assert
            Assert.Equal(orderDtoList.Count, result.Count);
            Assert.IsType<List<OrderDto>>(result);
        }
    }
}