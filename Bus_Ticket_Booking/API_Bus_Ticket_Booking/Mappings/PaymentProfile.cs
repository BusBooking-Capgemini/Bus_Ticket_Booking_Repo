using API_Bus_Ticket_Booking.DTOs.Payment;
using API_Bus_Ticket_Booking.Models;
using AutoMapper;

namespace API_Bus_Ticket_Booking.Mappings
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentResponseDto>()

                .ForMember(
                    dest => dest.TripId,
                    opt => opt.MapFrom(src =>
                        src.Booking != null &&
                        src.Booking.Trip != null
                            ? src.Booking.Trip.TripId
                            : 0))

                .ForMember(
                    dest => dest.SeatNumber,
                    opt => opt.MapFrom(src =>
                        src.Booking != null
                            ? src.Booking.SeatNumber
                            : 0))

                .ForMember(
                    dest => dest.FromCity,
                    opt => opt.MapFrom(src =>
                        src.Booking != null &&
                        src.Booking.Trip != null &&
                        src.Booking.Trip.Route != null
                            ? src.Booking.Trip.Route.FromCity
                            : ""))

                .ForMember(
                    dest => dest.ToCity,
                    opt => opt.MapFrom(src =>
                        src.Booking != null &&
                        src.Booking.Trip != null &&
                        src.Booking.Trip.Route != null
                            ? src.Booking.Trip.Route.ToCity
                            : ""))

                .ForMember(
                    dest => dest.TripDate,
                    opt => opt.MapFrom(src =>
                        src.Booking != null &&
                        src.Booking.Trip != null
                            ? src.Booking.Trip.TripDate
                            : DateTime.MinValue))

                .ForMember(
                    dest => dest.DepartureTime,
                    opt => opt.MapFrom(src =>
                        src.Booking != null &&
                        src.Booking.Trip != null
                            ? src.Booking.Trip.DepartureTime
                            : DateTime.MinValue))

                .ForMember(
                    dest => dest.ArrivalTime,
                    opt => opt.MapFrom(src =>
                        src.Booking != null &&
                        src.Booking.Trip != null
                            ? src.Booking.Trip.ArrivalTime
                            : DateTime.MinValue));
        }
    }
}