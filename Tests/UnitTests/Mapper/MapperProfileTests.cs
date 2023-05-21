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
        //Since we only have simple profiles, this is all we need for our exact case in terms of testing property mapping.
        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void ShouldMapOrderToOrderDto()
        {
            var order = new Order
            {
                OrderId = 1,
            };

            var orderDto = _mapper.Map<OrderDto>(order);

            Assert.IsType<OrderDto>(orderDto);
        }

        [Fact]
        public void ShouldMapOrderDetailToOrderDetailDto()
        {
            var orderDetail = new OrderDetail
            {
                OrderId = 1,
                ProductId = 1,
                UnitPrice = 1,
                Quantity = 1,
                Discount = 1
            };

            var orderDetailDto = _mapper.Map<OrderDetailDto>(orderDetail);

            Assert.IsType<OrderDetailDto>(orderDetailDto);
        }
    }
}
