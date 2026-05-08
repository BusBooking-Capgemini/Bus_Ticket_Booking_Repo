using API_Bus_Ticket_Booking.DTOs.Office;
using API_Bus_Ticket_Booking.Models;
using AutoMapper;

namespace API_Bus_Ticket_Booking.Mappings
{
    public class OfficeProfile : Profile
    {
        public OfficeProfile()
        {
            // AgencyOffice - OfficeResponseDto
            CreateMap<AgencyOffice, OfficeResponseDto>();

            // OfficeRequestDto - AgencyOffice
            CreateMap<OfficeRequestDto, AgencyOffice>();

            // AgencyOffice - OfficeRequestDto
            CreateMap<AgencyOffice, OfficeRequestDto>();
        }
    }
}
