using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Repositories;

public class ReviewRepository : IReviewRepository{
    private readonly BusTicketBookingContext _context;

    public ReviewRepository(BusTicketBookingContext context){
        _context = context;
    }

    public async Task<List<Review>> GetByCustomerIdAsync(int customerId){
        return await _context.Reviews.Where(r => r.CustomerId == customerId).Include(r => r.Customer).Include(r => r.Trip).ThenInclude(t => t.Route).OrderByDescending(r => r.ReviewDate).ToListAsync();
    }

    public async Task<Review?> GetByIdAsync(int reviewId){
        return await _context.Reviews.Include(r => r.Customer).Include(r => r.Trip).ThenInclude(t => t.Route).FirstOrDefaultAsync(r => r.ReviewId == reviewId);
    }

    public async Task<bool> CustomerAlreadyReviewedTripAsync(int customerId, int tripId){
        return await _context.Reviews.AnyAsync(r => r.CustomerId == customerId && r.TripId == tripId);
    }

    public async Task<int> GetNextReviewIdAsync(){
        // If any review exists, find the one with maximum 'ReviewId' + 1, else just return 1 meaning it is the first review being created.
        return await _context.Reviews.AnyAsync() ? 
        await _context.Reviews.MaxAsync(r => r.ReviewId) + 1
        : 1;
    }

    public async Task<Review> CreateAsync(Review review){
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
        return review;
    }

    public async Task UpdateAsync(Review review){
        _context.Reviews.Update(review);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Review review){
        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Review>> GetByTripIdAsync(int tripId){
        return await _context.Where(r => r.TripId == tripId).Include(r => r.Customer).OrderByDescending(r => r.ReviewDate).ToListAsync();
    }

    public async Task<double> GetAverageRatingForTripAsync(int tripId){
        // If any reviews exist for this trip exists, return the average otherwise just reutrn 0 signifying that there is no review for this trip.
        await _context.Reviews.Where(r => r.TripId == tripId).AnyAsync()
            ? await _context.Reviews.Where(r => r.TripId == tripId).AverageAsync(r => (double)r.Rating)
            : 0;
    }
}
