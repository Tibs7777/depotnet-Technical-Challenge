using Xunit;
using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;
using Services;using Repository;
using Microsoft.EntityFrameworkCore;
using RefactoringChallenge;
using Entities.Exceptions;
using Tests.UnitTests.Helpers;

//I am using an in memory db here for the tests, but there are limitations to this approach, and it is not something I would want in a full production app long-term.
//For a real test setup I would likely push for either a docker container or a a real db of some kind.
//In memory struggles with certain types of queries and is not a true representation of a real db.

namespace Tests.UnitTests.Services
{
    public class OrderServiceTests : IDisposable
    {
        private readonly DbContextOptions<RepositoryContext> _options;
        private readonly IMapper _mapper;
        private readonly OrderService _service;
        private RepositoryContext _repositoryContext;
        public OrderServiceTests()
        {
            _options = new DbContextOptionsBuilder<RepositoryContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            var context = new RepositoryContext(_options);
            _repositoryContext = context;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = configuration.CreateMapper();

            var repositoryManager = new RepositoryManager(context);

            _service = new OrderService(_mapper, repositoryManager);

            DatabaseSeeder.SeedDatabase(_repositoryContext);
        }

        public void Dispose()
        {
            _repositoryContext.Database.EnsureDeleted();
            _repositoryContext.Dispose();
        }

        [Fact]
        public async Task GetOrdersAsync_ValidPageParameters_ReturnsListOfOrders()
        {
            const int pageSize = 2;
            const int pageNumber = 1;

            List<OrderDto> result = await _service.GetOrdersAsync(pageSize, pageNumber);

            Assert.Equal(2, result.Count);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(101, 1)]
        [InlineData(1, 0)]
        public async Task GetOrdersAsync_ThrowsBadRequestException_WhenInvalidPageSizeOrPageNumber(int pageSize, int pageNumber)
        {
            await Assert.ThrowsAsync<BadRequestException>(() => _service.GetOrdersAsync(pageSize, pageNumber));
        }


        [Fact]
        public async Task GetOrderByIdAsync_ReturnsOrderDto()
        {
            const int orderId = 1;

            OrderDto result = await _service.GetOrderByIdAsync(orderId);

            Assert.NotNull(result);
            Assert.Equal(orderId, result.OrderId);
        }

        [Fact]
        public async Task GetOrderAndCheckIfExists_ThrowsNotFoundException()
        {
            const int notFoundOrderId = 100;

            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetOrderByIdAsync(notFoundOrderId));
        }

        [Fact]
        public async Task CreateOrderAsync_ValidOrderForCreation_ReturnsCreatedOrder()
        {
            OrderForCreationDto orderForCreationDto = new OrderForCreationDto
            {
                CustomerId = "VINET",
                EmployeeId = 1,
                RequiredDate = DateTime.Parse("2023-05-21T19:29:17.635Z"),
                ShipVia = 1,
                Freight = 1,
                ShipName = "string",
                ShipAddress = "string",
                ShipCity = "string",
                ShipRegion = "string",
                ShipPostalCode = "string",
                ShipCountry = "string",
                OrderDetails = new List<OrderDetailForCreationDto>
                {
                    new OrderDetailForCreationDto
                    {
                        ProductId = 1,
                        UnitPrice = 1,
                        Quantity = 1,
                        Discount = 0
                    }
                }
            };

            OrderDto result = await _service.CreateOrderAsync(orderForCreationDto);
            Order? dbResult = _repositoryContext.Orders.Find(result.OrderId);

            Assert.NotNull(dbResult);
            Assert.Equal(result.OrderId, dbResult.OrderId);
        }

        [Fact]
        public async Task CreateOrderDetailsByOrderIdAsync_ValidOrderForCreation_ReturnsCreatedOrderDetails()
        {
            var orderDetailsForCreation = new List<OrderDetailForCreationDto>()
            {
                new OrderDetailForCreationDto()
                {
                    ProductId = 40,
                    Discount = 0,
                    Quantity = 10,
                    UnitPrice = 10
                },
            };
            const int orderId = 1;

            List<OrderDetailDto> result = await _service.CreateOrderDetailsByOrderIdAsync(orderId, orderDetailsForCreation);
            Order? dbResult = _repositoryContext.Orders.Find(orderId);

            Assert.NotNull(dbResult);
            Assert.Equal(1, dbResult.OrderDetails.Count);
            Assert.Equal(40, dbResult.OrderDetails.First().ProductId);
        }

        [Fact]
        public async Task DeleteOrderByIdAsync_ValidOrderId_DeletesOrder()
        {
            const int orderId = 1;

            await _service.DeleteOrderByIdAsync(orderId);
            Order? dbResult = _repositoryContext.Orders.Find(orderId);

            Assert.Null(dbResult);
        }
    }
}