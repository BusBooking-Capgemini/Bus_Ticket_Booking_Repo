using API_Bus_Ticket_Booking.Models;
using AutoMapper;

public class BusProfile : Profile
{
    public BusProfile()
    {
        CreateMap<BusRequestDto, Bus>();

        CreateMap<Bus, BusResponseDto>()
            .ForMember(dest => dest.OfficeName,
                opt => opt.MapFrom(src => src.Office != null ? src.Office.OfficeContactPersonName: "N/A"));
    }
}
