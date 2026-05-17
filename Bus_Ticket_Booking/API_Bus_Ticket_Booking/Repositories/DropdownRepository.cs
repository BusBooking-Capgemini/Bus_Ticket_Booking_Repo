using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.DTOs.Common;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Repositories
{
    public class DropdownRepository
        : IDropdownRepository
    {
        private readonly BusTicketBookingContext
            _context;

        public DropdownRepository(
            BusTicketBookingContext context)
        {
            _context = context;
        }

        // =========================
        // ROUTES
        // =========================

        public async Task<List<DropdownDto>>
            GetRoutesDropdownAsync()
        {
            return await _context.Routes
                .Select(r =>
                    new DropdownDto
                    {
                        Id = r.RouteId,

                        Name =
                            r.FromCity +
                            " → " +
                            r.ToCity
                    })
                .ToListAsync();
        }

        // =========================
        // BUSES
        // =========================

        public async Task<List<DropdownDto>>
            GetBusesDropdownAsync()
        {
            return await _context.Buses
                .Select(b =>
                    new DropdownDto
                    {
                        Id = b.BusId,

                        Name =
                            b.Type +
                            " - " +
                            b.RegistrationNumber
                    })
                .ToListAsync();
        }

        // =========================
        // DRIVERS
        // =========================

        public async Task<List<DropdownDto>>
            GetDriversDropdownAsync()
        {
            return await _context.Drivers
                .Select(d =>
                    new DropdownDto
                    {
                        Id = d.DriverId,

                        Name = d.Name
                    })
                .ToListAsync();
        }

        // =========================
        // ADDRESSES
        // =========================

        public async Task<List<DropdownDto>>
            GetAddressesDropdownAsync()
        {
            return await _context.Addresses
                .Select(a =>
                    new DropdownDto
                    {
                        Id = a.AddressId,

                        Name = a.City
                    })
                .ToListAsync();
        }

        public async Task<List<DropdownDto>>
    GetBusesDropdownByOfficeAsync(
        int officeId)
        {
            return await _context.Buses
                .Where(b => b.OfficeId == officeId)
                .Select(b =>
                    new DropdownDto
                    {
                        Id = b.BusId,

                        Name =
                            b.Type +
                            " - " +
                            b.RegistrationNumber
                    })
                .ToListAsync();
        }

        public async Task<List<DropdownDto>>
    GetDriversDropdownByOfficeAsync(
        int officeId)
        {
            return await _context.Drivers
                .Where(d => d.OfficeId == officeId)
                .Select(d =>
                    new DropdownDto
                    {
                        Id = d.DriverId,

                        Name = d.Name
                    })
                .ToListAsync();
        }
    }
}