using AutoMapper;
using Entities.Models;
using RefactoringChallenge;
using Shared.DataTransferObjects;
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

        //Mapper config test will pick up on any errors in the profiles themselves. (e.g unmapped properties)
        //If any destination properties are unmapped, the test will fail.
        //Since we only have simple profiles, this is mostly all we need for our exact case in terms of testing property mapping.
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
            var order = _mapper.Map<Order>(orderForCreationDto);

            // Assert
            Assert.NotNull(order.OrderDate);

            var actualOrderDate = order.OrderDate.Value;
            var comparisonResult = DateTime.Compare(expectedOrderDate, actualOrderDate);
            Assert.InRange(comparisonResult, -10, 10);
        }
    }
}
