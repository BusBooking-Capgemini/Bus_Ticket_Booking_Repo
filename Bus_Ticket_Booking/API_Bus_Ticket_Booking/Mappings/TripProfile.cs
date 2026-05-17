using API_Bus_Ticket_Booking.DTOs.Trip;
using API_Bus_Ticket_Booking.Models;
using AutoMapper;

namespace API_Bus_Ticket_Booking.Mappings
{
    public class TripProfile : Profile
    {
        public TripProfile()
        {
            CreateMap<Trip, TripResponseDto>()

                .ForMember(
                    dest => dest.FromCity,
                    opt => opt.MapFrom(
                        src => src.Route != null
                            ? src.Route.FromCity
                            : ""))

                .ForMember(
                    dest => dest.ToCity,
                    opt => opt.MapFrom(
                        src => src.Route != null
                            ? src.Route.ToCity
                            : ""))

                .ForMember(
                    dest => dest.BusType,
                    opt => opt.MapFrom(
                        src => src.Bus != null
                            ? src.Bus.Type
                            : ""))

                .ForMember(
                    dest => dest.BoardingAddressId,
                    opt => opt.MapFrom(
                        src => src.BoardingAddressId))

                .ForMember(
                    dest => dest.DroppingAddressId,
                    opt => opt.MapFrom(
                        src => src.DroppingAddressId))

                .ForMember(
                    dest => dest.BoardingCity,
                    opt => opt.MapFrom(
                        src => src.BoardingAddressId.ToString()))

                .ForMember(
                    dest => dest.DroppingCity,
                    opt => opt.MapFrom(
                        src => src.DroppingAddressId.ToString()))

                .ForMember(
                    dest => dest.Driver1DriverId,
                    opt => opt.MapFrom(
                        src => src.Driver1DriverId))

                .ForMember(
                    dest => dest.Driver2DriverId,
                    opt => opt.MapFrom(
                        src => src.Driver2DriverId))

                .ForMember(
                    dest => dest.Driver1Name,
                    opt => opt.MapFrom(
                        src => src.Driver1Driver != null
                            ? src.Driver1Driver.Name
                            : ""))

                .ForMember(
                    dest => dest.Driver2Name,
                    opt => opt.MapFrom(
                        src => src.Driver2Driver != null
                            ? src.Driver2Driver.Name
                            : ""));

            CreateMap<CreateTripDto, Trip>();
        }
    }
}