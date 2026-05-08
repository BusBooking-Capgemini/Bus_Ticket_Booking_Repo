using API_Bus_Ticket_Booking.DTOs;
using API_Bus_Ticket_Booking.DTOs.Bus;
using API_Bus_Ticket_Booking.DTOs.Driver;
using API_Bus_Ticket_Booking.Models;
using AutoMapper;

namespace API_Bus_Ticket_Booking.Mappings;

public class BusProfile : Profile
{
    public BusProfile()
    {
        CreateMap<BusRequestDto, Bus>();

        CreateMap<Bus, BusResponseDto>()
            .ForMember(
                dest => dest.OfficeName,
                opt =>
                    opt.MapFrom(src =>
                        src.Office != null ? src.Office.OfficeContactPersonName : "N/A"
                    )
            );
    }
}
