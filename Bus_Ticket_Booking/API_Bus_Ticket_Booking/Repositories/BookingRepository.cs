using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Repositories;

public class BookingRepository : IBookingRepository{
    private readonly BusTicketBookingContext _context;

    public BookingRepository(BusTicketBookingContext context){
        _context = context;
    }

    public async Task<List<Payment>> GetBookingsByCustomerIdAsync(int customerId){
        return await _context.Payments.Where(p => p.CustomerId == customerId).Include(p => p.Booking).ThenInclude(b => b.Trip).ThenInclude(t => t.Route).Include(p => p.Booking.Trip.Bus).OrderByDescending(p => p.Booking.Trip.TripDate).ToListAsync();
    }

    public async Task<Payment?> GetBookingDetailAsync(int customerId, int bookingId){
        return await _context.Payments.Where(p => p.CustomerId == customerId && p.BookingId == bookingId).Include(p => p.Booking).ThenInclude(b => b.Trip).ThenInclude(t => t.Route).FirstOrDefaultAsync();
    }

    public async Task<bool> CustomerHasBookedTripAsync(int customerId, int tripId){
        return await _context.Payments.Where(p => p.CustomerId == customerId && p.Booking.TripId == tripId && p.PaymentStatus == "Success");
    }

    public async Task<Booking?> GetByIdAsync(int bookingId){
        return await _context.Bookings
            .Include(b => b.Trip)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);
    }

    public async Task<List<Booking>> GetAvailableSeatsByTripAsync(int tripId){
        return await _context.Bookings.Where(b => b.TripId == tripId && b.Status == "Available").ToListAsync();
    }

    public async Task<Booking> CreateAsync(Booking booking){
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return booking;
    }

    public async Task<Payment> CreatePaymentAsync(Payment payment){
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }
    
    public async Task UpdateAsync(Booking booking){
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
    }
}