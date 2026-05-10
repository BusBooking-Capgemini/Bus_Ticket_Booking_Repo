using API_Bus_Ticket_Booking.DTOs.Booking;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services.Interfaces;
using AutoMapper;



namespace API_Bus_Ticket_Booking.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepo;
    private readonly ICustomerRepository _customerRepo;

    public BookingService(IBookingRepository bookingRepo, ICustomerRepository customerRepo)
    {
        _bookingRepo = bookingRepo;
        _customerRepo = customerRepo;
    }

    public async Task<List<BookingResponseDto>> GetCustomerBookingsAsync(int customerId)
    {
        var payments = await _bookingRepo.GetBookingsByCustomerIdAsync(customerId);

        return payments.Select(MapToDto).ToList();
    }

    public async Task<BookingResponseDto?> GetBookingDetailAsync(int customerId, int bookingId)
    {
        var payment = await _bookingRepo.GetBookingDetailAsync(customerId, bookingId);

        if (payment == null)
            return null;

        return MapToDto(payment);
    }

    public async Task<(bool success, string message, int? bookingId)> BookSeatAsync(
        int customerId,
        int tripId,
        int seatNumber
    )
    {
        // Check customer exists
        var customer = await _customerRepo.ExistsAsync(customerId);

        if (customer == false)
            return (false, "Customer not found", null);

        // check if seat is available
        var available = await _bookingRepo.GetAvailableSeatsByTripAsync(tripId);

        var seat = available.FirstOrDefault(s => s.SeatNumber == seatNumber);

        if (seat == null)
            return (false, $"Seat {seatNumber} is not available for this trip.", null);

        // Mark seat as booked
        seat.Status = "Booked";
        await _bookingRepo.UpdateAsync(seat);

        // Create payment record (pending — real payment flow would be separate)
        var payment = new Payment
        {
            BookingId = seat.BookingId,
            CustomerId = customerId,
            Amount = null, // Set by payment step
            PaymentStatus = "Failed", // Will be updated on successful payment
            PaymentDate = DateTime.UtcNow,
        };
        await _bookingRepo.CreatePaymentAsync(payment);

        return (true, "Seat booked successfully. Proceed to payment.", seat.BookingId);
    }

    public async Task<List<int>> GetAvailableSeatsAsync(int tripId)
    {
        var seats = await _bookingRepo.GetAvailableSeatsByTripAsync(tripId);

        return seats.Select(s => s.SeatNumber).OrderBy(s => s).ToList();
    }

    public async Task<(bool success, string message)> CancelBookingAsync(
        int customerId,
        int bookingId
    )
    {
        var payment = await _bookingRepo.GetBookingDetailAsync(customerId, bookingId);

        if (payment == null)
            return (false, "Booking not found for this customer.");

        if (payment.Booking.Trip.TripDate <= DateTime.UtcNow)
            return (false, "Cannot cancel a booking for a past or ongoing trip.");

        // Free the seat back
        payment.Booking.Status = "Available";

        await _bookingRepo.UpdateAsync(payment.Booking);

        return (true, "Booking cancelled successfully.");
    }

    private static BookingResponseDto MapToDto(Payment p) =>
        new()
        {
            BookingId = p.Booking.BookingId,
            TripId = p.Booking.TripId ?? 0,
            FromCity = p.Booking.Trip.Route.FromCity,
            ToCity = p.Booking.Trip.Route.ToCity,
            TripDate = p.Booking.Trip.TripDate,
            DepartureTime = p.Booking.Trip.DepartureTime,
            ArrivalTime = p.Booking.Trip.ArrivalTime,
            SeatNumber = p.Booking.SeatNumber,
            Status = p.Booking.Status,
            Fare = p.Booking.Trip.Fare,
            AmountPaid = p.Amount,
            PaymentStatus = p.PaymentStatus,
        };
}




// -----------------------------------------------------------------------------
// -----------------------------------------------------------------------------



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
                            .GetActiveBookingsByOfficeAsync(officeId.Value)
                };
            }

            return new BookingDashboardDto
            {
                TotalBookings =
                    await _bookingRepository
                        .GetTotalBookingsByAgencyAsync(agencyId),

                ActiveBookings =
                    await _bookingRepository
                        .GetActiveBookingsByAgencyAsync(agencyId)
            };
        }

        // Analytics
        public async Task<BookingAnalyticsDto> GetAnalyticsAsync(
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

            return new BookingAnalyticsDto
            {
                OccupancyRate = occupancyRate,

                MostBookedTripId = mostBookedTripId,

                MostPopularRoute = mostPopularRoute
            };
        }
    }
}

