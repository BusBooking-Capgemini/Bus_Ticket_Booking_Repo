using API_Bus_Ticket_Booking.DTOs.Agency;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services.Interfaces;
using AutoMapper;
using System.Security.Cryptography.Xml;

namespace API_Bus_Ticket_Booking.Services
{
    public class AgencyService : IAgencyService
    {
        private readonly IAgencyRepository _repository;

        private readonly IMapper _mapper;

        public AgencyService(IAgencyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AgencyResponseDto>> GetAllAsync()
        {
            var agencies = await _repository.GetAllAsync();

            if (!agencies.Any())
                throw new NotFoundException("No agencies found");

            return _mapper.Map<IEnumerable<AgencyResponseDto>>(agencies);
        }

        public async Task<AgencyResponseDto> GetByIdAsync(int id)
        {
            var agency = await _repository.GetByIdAsync(id);

            if (agency == null)
                throw new NotFoundException("Agency not found");

            return _mapper.Map<AgencyResponseDto>(agency);
        }

        public async Task UpdateAsync(int id, AgencyRequestDto dto)
        {
            var agency = await _repository.GetByIdAsync(id);

            if (agency == null)
                throw new NotFoundException("Agency not found");

            agency.Name = dto.Name;
            agency.ContactPersonName = dto.ContactPersonName;
            agency.Email = dto.Email;
            agency.Phone = dto.Phone;

            await _repository.UpdateAsync(agency);
        }

        public async Task DeleteAsync(int id)
        {
            var agency = await _repository.GetByIdAsync(id);

            if (agency == null)
                throw new NotFoundException("Agency not found");

            await _repository.DeleteAsync(agency);
        }

        public async Task<IEnumerable<object>> GetAgencyOfficesAsync(int agencyId)
        {
            var agency = await _repository.GetByIdAsync(agencyId);

            if (agency == null)
                throw new NotFoundException("Agency not found");

            var offices = await _repository.GetAgencyOfficesAsync(agencyId);

            if (!offices.Any())
                throw new NotFoundException("No offices found");

            // why select -> to transform each item in a collection into another shape

            return offices.Select(x => new
            {
                x.OfficeId,
                x.OfficeMail,
                x.OfficeContactPersonName,
                x.OfficeContactNumber
            });
        }

        public async Task<object> GetAgencySummaryAsync(int agencyId)
        {
            var agency = await _repository.GetByIdAsync(agencyId);

            if (agency == null)
                throw new NotFoundException("Agency not found");

            var offices = await _repository.GetAgencyOfficesAsync(agencyId);

            // why new -> to create a new object containing only these properties

            return new
            {
                AgencyId = agencyId,
                TotalOffices = offices.Count()
            };
        }

        public async Task<IEnumerable<object>> GetOfficeBookingsAsync(int agencyId, int officeId)
        {
            var bookings = await _repository.GetOfficeBookingsAsync(agencyId, officeId);

            if (!bookings.Any())
                throw new NotFoundException("No bookings found");

            return bookings.Select(x => new
            {
                x.BookingId,
                x.SeatNumber,
                x.Status
            });
        }

        public async Task<IEnumerable<object>> GetOfficePaymentsAsync(int agencyId, int officeId)
        {
            var payments = await _repository.GetOfficePaymentsAsync(agencyId, officeId);

            if (!payments.Any())
                throw new NotFoundException("No payments found");

            return payments.Select(x => new
            {
                x.PaymentId,
                x.Amount,
                x.PaymentStatus
            });
        }

        public async Task<IEnumerable<object>> GetOfficeTripsAsync(int agencyId, int officeId)
        {
            var trips = await _repository.GetOfficeTripsAsync(agencyId, officeId);

            if (!trips.Any())
                throw new NotFoundException("No trips found");

            return trips.Select(x => new
            {
                x.TripId,
                x.DepartureTime,
                x.ArrivalTime,
                x.Fare
            });
        }

        public async Task<IEnumerable<object>> GetOfficeBusesAsync(int agencyId, int officeId)
        {
            var buses = await _repository.GetOfficeBusesAsync(agencyId, officeId);

            if (!buses.Any())
                throw new NotFoundException("No buses found");

            return buses.Select(x => new
            {
                x.BusId,
                x.RegistrationNumber,
                x.Capacity,
                x.Type
            });
        }

        public async Task<IEnumerable<object>> GetOfficeDriversAsync(int agencyId, int officeId)
        {
            var drivers = await _repository.GetOfficeDriversAsync(agencyId, officeId);

            if (!drivers.Any())
                throw new NotFoundException("No drivers found");

            return drivers.Select(x => new
            {
                x.DriverId,
                x.Name,
                x.Phone,
                x.LicenseNumber
            });
        }
    }
}