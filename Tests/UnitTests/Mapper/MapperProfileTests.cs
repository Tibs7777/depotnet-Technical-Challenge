using AutoMapper;
using Entities.Models;
using RefactoringChallenge;
using Xunit;

namespace Tests.UnitTests.Mapper
{
    public class MapperProfileTests
    {
        private readonly IMapper _mapper;

        public MapperProfileTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void Map_OrderForCreationDto_To_Order_With_OrderDate_As_UtcNow()
        {
            // Arrange
            var expectedOrderDate = DateTime.UtcNow;

            var orderForCreationDto = new OrderForCreationDto();

            // Act
            Order order = _mapper.Map<Order>(orderForCreationDto);

            // Assert
            Assert.NotNull(order.OrderDate);

            DateTime actualOrderDate = order.OrderDate.Value;
            int comparisonResult = DateTime.Compare(expectedOrderDate, actualOrderDate);
            Assert.InRange(comparisonResult, -10, 10);
        }
    }
}
