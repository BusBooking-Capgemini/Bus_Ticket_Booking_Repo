using API_Bus_Ticket_Booking.DTOs.Booking;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services.Interfaces;
using AutoMapper;

namespace API_Bus_Ticket_Booking.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        private readonly IMapper _mapper;

        public BookingService(
            IBookingRepository bookingRepository,
            IMapper mapper)
        {
            _bookingRepository = bookingRepository;

            _mapper = mapper;
        }

        public async Task<BookingResponseDto>
            CreateBookingAsync(
                CreateBookingDto dto)
        {
            var booking = new Booking
            {
                TripId = dto.TripId,

                SeatNumber = dto.SeatNumber,

                Status = "Booked"
            };

            var createdBooking =
                await _bookingRepository
                    .CreateBookingAsync(booking);

            if (createdBooking == null)
            {
                throw new NotFoundException(
                    "Seat not found");
            }

            return MapBooking(createdBooking);
        }

        public async Task CancelBookingAsync(
            int bookingId)
        {
            var booking =
                await _bookingRepository
                    .GetBookingByIdAsync(
                        bookingId);

            if (booking == null)
            {
                throw new NotFoundException(
                    "Booking not found");
            }

            if (booking.Status == "Available")
            {
                throw new ConflictException(
                    "Seat already available");
            }

            await _bookingRepository
                .CancelBookingAsync(booking);
        }

        public async Task<BookingResponseDto>
            GetBookingByIdAsync(
                int bookingId)
        {
            var booking =
                await _bookingRepository
                    .GetBookingByIdAsync(
                        bookingId);

            if (booking == null)
            {
                return null!;
            }

            return MapBooking(booking);
        }

        public async Task<
            IEnumerable<BookingResponseDto>>
            GetCustomerBookingsAsync(
                int customerId)
        {
            var bookings =
                await _bookingRepository
                    .GetCustomerBookingsAsync(
                        customerId);

            return bookings.Select(MapBooking);
        }

        public async Task<
            IEnumerable<BookingResponseDto>>
            GetOfficeBookingsAsync(
                int officeId)
        {
            var bookings =
                await _bookingRepository
                    .GetOfficeBookingsAsync(
                        officeId);

            return bookings.Select(MapBooking);
        }

        public async Task<
            IEnumerable<BookingResponseDto>>
            GetAgencyBookingsAsync(
                int agencyId)
        {
            var bookings =
                await _bookingRepository
                    .GetAgencyBookingsAsync(
                        agencyId);

            return bookings.Select(MapBooking);
        }

        public async Task<
            IEnumerable<BookingResponseDto>>
            GetBookingsByTripAsync(
                int tripId)
        {
            var bookings =
                await _bookingRepository
                    .GetBookingsByTripAsync(
                        tripId);

            return bookings.Select(MapBooking);
        }

        private BookingResponseDto
            MapBooking(
                Booking b)
        {
            return new BookingResponseDto
            {
                BookingId =
                    b.BookingId,

                TripId =
                    b.TripId ?? 0,

                SeatNumber =
                    b.SeatNumber,

                Status =
                    b.Status,

                FromCity =
                    b.Trip?.Route?.FromCity
                        ?? "",

                ToCity =
                    b.Trip?.Route?.ToCity
                        ?? "",

                TripDate =
                    b.Trip?.TripDate
                        ?? DateTime.MinValue,

                DepartureTime =
                    b.Trip?.DepartureTime
                        ?? DateTime.MinValue,

                ArrivalTime =
                    b.Trip?.ArrivalTime
                        ?? DateTime.MinValue,

                Fare =
                    b.Trip?.Fare
                        ?? 0,

                PaymentStatus =
                    b.Payments
                        .FirstOrDefault()?.PaymentStatus
                        ?? "Pending",

                PaidAmount =
                    b.Payments
                        .FirstOrDefault()?.Amount
                        ?? 0
            };
        }

        public async Task<BookingDashboardDto>
            GetDashboardAsync(
                int agencyId,
                int? officeId)
        {
            if (officeId.HasValue)
            {
                return new BookingDashboardDto
                {
                    TotalBookings =
                        await _bookingRepository
                            .GetTotalBookingsByOfficeAsync(
                                officeId.Value),

                    ActiveBookings =
                        await _bookingRepository
                            .GetActiveBookingsByOfficeAsync(
                                officeId.Value)
                };
            }

            return new BookingDashboardDto
            {
                TotalBookings =
                    await _bookingRepository
                        .GetTotalBookingsByAgencyAsync(
                            agencyId),

                ActiveBookings =
                    await _bookingRepository
                        .GetActiveBookingsByAgencyAsync(
                            agencyId)
            };
        }

        public async Task<BookingAnalyticsDto>
            GetAnalyticsAsync(
                int agencyId,
                int? officeId)
        {
            double occupancyRate;

            int mostBookedTripId;

            string mostPopularRoute;

            if (officeId.HasValue)
            {
                occupancyRate =
                    await _bookingRepository
                        .GetOccupancyRateByOfficeAsync(
                            officeId.Value);

                mostBookedTripId =
                    await _bookingRepository
                        .GetMostBookedTripByOfficeAsync(
                            officeId.Value);

                mostPopularRoute =
                    await _bookingRepository
                        .GetMostPopularRouteByOfficeAsync(
                            officeId.Value);
            }
            else
            {
                occupancyRate =
                    await _bookingRepository
                        .GetOccupancyRateByAgencyAsync(
                            agencyId);

                mostBookedTripId =
                    await _bookingRepository
                        .GetMostBookedTripByAgencyAsync(
                            agencyId);

                mostPopularRoute =
                    await _bookingRepository
                        .GetMostPopularRouteByAgencyAsync(
                            agencyId);
            }

            return new BookingAnalyticsDto
            {
                OccupancyRate = occupancyRate,

                MostBookedTripId = mostBookedTripId,

                MostPopularRoute = mostPopularRoute
            };
        }
    }
}