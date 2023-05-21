using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace RefactoringChallenge
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderDto>();

            CreateMap<OrderDetail, OrderDetailDto>();
        }
    }
}
