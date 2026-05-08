using API_Bus_Ticket_Booking.DTOs.Agency;
using API_Bus_Ticket_Booking.Models;
using AutoMapper;

namespace API_Bus_Ticket_Booking.Mappings
{
    public class AgencyProfile : Profile
    {
        public AgencyProfile()
        {
            // Agency - AgencyResponseDto
            CreateMap<Agency, AgencyResponseDto>().ForMember(
                dest => dest.ContactPersonName,
                opt => opt.MapFrom(
                    src => src.ContactPersonName)
            );

            // AgencyRequestDto - Agency
            CreateMap<AgencyRequestDto, Agency>().ForMember(
                dest => dest.ContactPersonName,
                opt => opt.MapFrom(
                    src => src.ContactPersonName)
            );

            // Agency - AgencyRequestDto
            CreateMap<Agency, AgencyRequestDto>().ForMember(
                dest => dest.ContactPersonName,
                opt => opt.MapFrom(
                    src => src.ContactPersonName)
            );
        }
    }
}
