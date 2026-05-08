using API_Bus_Ticket_Booking.DTOs;
using API_Bus_Ticket_Booking.DTOs.Bus;
using API_Bus_Ticket_Booking.DTOs.Driver;
using API_Bus_Ticket_Booking.Models;
using AutoMapper;

namespace API_Bus_Ticket_Booking.Mappings;

public class DriverProfile : Profile
{
    public DriverProfile()
    {
        CreateMap<DriverRequestDto, Driver>()
            .ForMember(dest => dest.AddressId, opt => opt.Ignore())
            .ForMember(dest => dest.Address, opt => opt.Ignore());

        CreateMap<Driver, DriverResponseDto>()
            .ForMember(
                dest => dest.OfficeName,
                opt =>
                    opt.MapFrom(src =>
                        src.Office != null ? src.Office.OfficeContactPersonName : "N/A"
                    )
            )
            .ForMember(
                dest => dest.Address,
                opt => opt.MapFrom(src => src.Address != null ? src.Address.Address1 : "")
            )
            .ForMember(
                dest => dest.City,
                opt => opt.MapFrom(src => src.Address != null ? src.Address.City : "")
            )
            .ForMember(
                dest => dest.State,
                opt => opt.MapFrom(src => src.Address != null ? src.Address.State : "")
            )
            .ForMember(
                dest => dest.ZipCode,
                opt => opt.MapFrom(src => src.Address != null ? src.Address.ZipCode : "")
            );
    }
}
