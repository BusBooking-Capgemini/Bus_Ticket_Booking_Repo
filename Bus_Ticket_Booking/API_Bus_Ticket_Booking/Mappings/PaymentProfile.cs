using API_Bus_Ticket_Booking.DTOs.Payment;
using API_Bus_Ticket_Booking.Models;
using AutoMapper;

namespace API_Bus_Ticket_Booking.Mappings
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentResponseDto>();

            CreateMap<CreatePaymentDto, Payment>();
        }
    }
}