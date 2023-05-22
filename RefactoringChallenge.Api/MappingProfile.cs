using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;
using System;

namespace RefactoringChallenge
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderDto>();

            CreateMap<OrderDetail, OrderDetailDto>();

            //This part should be done in the db as a default value, but for now I have kept it here
            CreateMap<OrderForCreationDto, Order>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.ShippedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Employee, opt => opt.Ignore())
                .ForMember(dest => dest.ShipViaNavigation, opt => opt.Ignore());            

            CreateMap<OrderDetailForCreationDto, OrderDetail>()
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());
        }
    }
}
