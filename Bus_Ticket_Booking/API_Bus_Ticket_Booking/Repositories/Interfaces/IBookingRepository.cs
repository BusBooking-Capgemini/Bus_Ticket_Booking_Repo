using API_Bus_Ticket_Booking.Models;

namespace API_Bus_Ticket_Booking.Repositories.Interfaces;

public interface IBookingRepository
{
    Task<List<Payment>> GetBookingsByCustomerIdAsync(int customerId);
    Task<Payment?> GetBookingDetailAsync(int customerId, int bookingId);
    Task<bool> CustomerHasBookedTripAsync(int customerId, int tripId);
    Task<Booking?> GetByIdAsync(int bookingId);
    Task<List<Booking>> GetAvailableSeatsByTripAsync(int tripId);
    Task<Booking> CreateAsync(Booking booking);
    Task<Payment> CreatePaymentAsync(Payment payment);
    Task UpdateAsync(Booking booking);
}