using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.DTOs.Office;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;


namespace API_Bus_Ticket_Booking.Services
{
    public class OfficeService : IOfficeService
    {
        private readonly IOfficeRepository _repository;

        private readonly IMapper _mapper;

        private readonly BusTicketBookingContext _context;

        public OfficeService(IOfficeRepository repository, IMapper mapper, BusTicketBookingContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<OfficeResponseDto>> GetAllAsync()
        {
            var offices = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<OfficeResponseDto>>(offices);
        }

        public async Task<OfficeResponseDto> GetByIdAsync(int id)
        {
            var office = await _repository.GetByIdAsync(id);

            if (office == null)
                throw new NotFoundException("Office not found");

            return _mapper.Map<OfficeResponseDto>(office);
        }

        public async Task<OfficeResponseDto> CreateAsync(
    OfficeRequestDto dto)
        {
            var agency = await _context.Agencies.FindAsync(dto.AgencyId);

            if (agency == null)
                throw new NotFoundException("Agency not found");

            var address = await _context.Addresses.FindAsync(dto.OfficeAddressId);

            if (address == null)
                throw new NotFoundException("Address not found");

            var office = _mapper.Map<AgencyOffice>(dto);

            await _repository.AddAsync(office);

            return _mapper.Map<OfficeResponseDto>(office);
        }

        public async Task UpdateAsync(int id, OfficeRequestDto dto)
        {
            var office = await _repository.GetByIdAsync(id);

            if (office == null)
                throw new NotFoundException("Office not found");

            var address = await _context.Addresses.FindAsync(dto.OfficeAddressId);

            if (address == null)
                throw new NotFoundException("Address not found");

            office.OfficeMail = dto.OfficeMail;
            office.OfficeContactPersonName = dto.OfficeContactPersonName;

            office.OfficeContactNumber = dto.OfficeContactNumber;

            office.OfficeAddressId = dto.OfficeAddressId;

            await _repository.UpdateAsync(office);
        }

        public async Task DeleteAsync(int id)
        {
            var office = await _repository.GetByIdAsync(id);

            if (office == null)
                throw new NotFoundException("Office not found");

            await _repository.DeleteAsync(office);
        }

        public async Task<object> GetSummaryAsync(int officeId)
        {
            var buses = await _repository.GetBusesAsync(officeId);

            var drivers = await _repository.GetDriversAsync(officeId);

            return new
            {
                OfficeId = officeId,
                TotalBuses = buses.Count(),
                TotalDrivers = drivers.Count()
            };
        }

        public async Task<IEnumerable<object>> GetBusesAsync(int officeId)
        {
            return (await _repository.GetBusesAsync(officeId))
                .Select(x => new
                {
                    x.BusId,
                    x.RegistrationNumber,
                    x.Capacity,
                    x.Type
                });
        }

        public async Task<IEnumerable<object>> GetDriversAsync(int officeId)
        {
            return (await _repository.GetDriversAsync(officeId))
                .Select(x => new
                {
                    x.DriverId,
                    x.Name,
                    x.Phone
                });
        }

        public async Task<IEnumerable<object>> GetTripsAsync(int officeId)
        {
            return (await _repository.GetTripsAsync(officeId))
                .Select(x => new
                {
                    x.TripId,
                    x.DepartureTime,
                    x.ArrivalTime,
                    x.Fare
                });
        }

        public async Task<IEnumerable<object>> GetBookingsAsync(int officeId)
        {
            return (await _repository.GetBookingsAsync(officeId))
                .Select(x => new
                {
                    x.BookingId,
                    x.SeatNumber,
                    x.Status
                });
        }

        public async Task<IEnumerable<object>> GetPaymentsAsync(int officeId)
        {
            return (await _repository.GetPaymentsAsync(officeId))
                .Select(x => new
                {
                    x.PaymentId,
                    x.Amount,
                    x.PaymentStatus
                });
        }
    }
}
