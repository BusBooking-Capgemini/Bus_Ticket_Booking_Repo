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

        // Create Booking
        public async Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto dto)
        {
            var booking = new Booking
            {
                TripId = dto.TripId,
                SeatNumber = dto.SeatNumber,
                Status = "Booked"
            };

            var createdBooking =
                await _bookingRepository.CreateBookingAsync(booking);

            if (createdBooking == null)
            {
                throw new NotFoundException("Seat not found");
            }

            return _mapper.Map<BookingResponseDto>(createdBooking);
        }

        // Cancel Booking
        public async Task CancelBookingAsync(int bookingId)
        {
            var booking =
                await _bookingRepository.GetBookingByIdAsync(bookingId);

            if (booking == null)
            {
                throw new NotFoundException("Booking not found");
            }

            if (booking.Status == "Available")
            {
                throw new ConflictException("Seat already available");
            }

            if (booking.Trip != null)
            {
                var departureTime = booking.Trip.DepartureTime;

                if (departureTime <= DateTime.Now.AddMinutes(30))
                {
                    throw new BusinessException(
                        "Booking can only be cancelled before 30 minutes of departure");
                }
            }


            await _bookingRepository.CancelBookingAsync(booking);
        }

        // Get Booking By Id
        public async Task<BookingResponseDto> GetBookingByIdAsync(int bookingId)
        {
            var booking =
                await _bookingRepository.GetBookingByIdAsync(bookingId);

            if (booking == null)
            {
                throw new NotFoundException("Booking not found");
            }

            return _mapper.Map<BookingResponseDto>(booking);
        }

        // Customer Bookings
        public async Task<IEnumerable<BookingResponseDto>> GetCustomerBookingsAsync(int customerId)
        {
            var bookings =
                await _bookingRepository.GetCustomerBookingsAsync(customerId);

            return _mapper.Map<IEnumerable<BookingResponseDto>>(bookings);
        }

        // Office Bookings
        public async Task<IEnumerable<BookingResponseDto>> GetOfficeBookingsAsync(int officeId)
        {
            var bookings =
                await _bookingRepository.GetOfficeBookingsAsync(officeId);

            return _mapper.Map<IEnumerable<BookingResponseDto>>(bookings);
        }

        // Agency Bookings
        public async Task<IEnumerable<BookingResponseDto>> GetAgencyBookingsAsync(int agencyId)
        {
            var bookings =
                await _bookingRepository.GetAgencyBookingsAsync(agencyId);

            return _mapper.Map<IEnumerable<BookingResponseDto>>(bookings);
        }

        // Dashboard
        public async Task<BookingDashboardDto> GetDashboardAsync(
            int agencyId,
            int? officeId)
        {
            if (officeId.HasValue)
            {
                return new BookingDashboardDto
                {
                    TotalBookings =
                        await _bookingRepository
                            .GetTotalBookingsByOfficeAsync(officeId.Value),

                    ActiveBookings =
                        await _bookingRepository
                            .GetActiveBookingsByOfficeAsync(officeId.Value),

                    CancelledBookings = 0,

                    CompletedBookings = 0
                };
            }

            return new BookingDashboardDto
            {
                TotalBookings =
                    await _bookingRepository
                        .GetTotalBookingsByAgencyAsync(agencyId),

                ActiveBookings =
                    await _bookingRepository
                        .GetActiveBookingsByAgencyAsync(agencyId),

                CancelledBookings = 0,

                CompletedBookings = 0
            };
        }

        // Analytics
        public async Task<BookingAnalyticsDto> GetAnalyticsAsync(
            int agencyId,
            int? officeId)
        {
            int totalBookings;

            int cancelledBookings = 0;

            double occupancyRate;

            int mostBookedTripId;

            string mostPopularRoute;

            if (officeId.HasValue)
            {
                totalBookings =
                    await _bookingRepository
                        .GetTotalBookingsByOfficeAsync(officeId.Value);

                occupancyRate =
                    await _bookingRepository
                        .GetOccupancyRateByOfficeAsync(officeId.Value);

                mostBookedTripId =
                    await _bookingRepository
                        .GetMostBookedTripByOfficeAsync(officeId.Value);

                mostPopularRoute =
                    await _bookingRepository
                        .GetMostPopularRouteByOfficeAsync(officeId.Value);
            }
            else
            {
                totalBookings =
                    await _bookingRepository
                        .GetTotalBookingsByAgencyAsync(agencyId);

                occupancyRate =
                    await _bookingRepository
                        .GetOccupancyRateByAgencyAsync(agencyId);

                mostBookedTripId =
                    await _bookingRepository
                        .GetMostBookedTripByAgencyAsync(agencyId);

                mostPopularRoute =
                    await _bookingRepository
                        .GetMostPopularRouteByAgencyAsync(agencyId);
            }

            double cancellationRate = 0;

            if (totalBookings > 0)
            {
                cancellationRate =
                    ((double)cancelledBookings / totalBookings) * 100;
            }

            return new BookingAnalyticsDto
            {
                CancellationRate = cancellationRate,

                OccupancyRate = occupancyRate,

                MostBookedTripId = mostBookedTripId,

                MostPopularRoute = mostPopularRoute
            };
        }
    }
}