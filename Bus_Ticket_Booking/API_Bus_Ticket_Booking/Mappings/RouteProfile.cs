using API_Bus_Ticket_Booking.DTOs.Route;
using API_Bus_Ticket_Booking.Models;
using AutoMapper;

namespace API_Bus_Ticket_Booking.Mappings
{
    public class RouteProfile : Profile
    {
        public RouteProfile()
        {
            CreateMap<API_Bus_Ticket_Booking.Models.Route, RouteresponseDto>();

            CreateMap<CreateRouteDto, API_Bus_Ticket_Booking.Models.Route>();

            CreateMap<UpdateRouteDto, API_Bus_Ticket_Booking.Models.Route>()
                .ForAllMembers(opt => opt.Condition(
                    (src, dest, srcMember) => srcMember != null));
        }
    }
}
