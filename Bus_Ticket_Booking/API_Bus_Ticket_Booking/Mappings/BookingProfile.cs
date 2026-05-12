using API_Bus_Ticket_Booking.DTOs.Booking;
using API_Bus_Ticket_Booking.Models;
using AutoMapper;

namespace API_Bus_Ticket_Booking.Mappings
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingResponseDto>();

            CreateMap<CreateBookingDto, Booking>();
        }
    }
}