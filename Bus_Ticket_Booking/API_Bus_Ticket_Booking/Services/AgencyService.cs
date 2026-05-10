using Microsoft.EntityFrameworkCore;
using API_Bus_Ticket_Booking.Data;
using API_Bus_Ticket_Booking.Models;
using API_Bus_Ticket_Booking.DTOs.Agency;
using API_Bus_Ticket_Booking.Exceptions;
using API_Bus_Ticket_Booking.Repositories.Interfaces;
using API_Bus_Ticket_Booking.Services.Interfaces;
using AutoMapper;

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

            return _mapper.Map<IEnumerable<AgencyResponseDto>>(agencies);
        }

        public async Task<AgencyResponseDto> GetByIdAsync(int id)
        {
            var agency = await _repository.GetByIdAsync(id);

            if (agency == null)
                throw new NotFoundException("Agency not found");

            return _mapper.Map<AgencyResponseDto>(agency);
        }

        public async Task UpdateAsync(
            int id,
            AgencyRequestDto dto)
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
            return (await _repository.GetAgencyOfficesAsync(agencyId))
                .Select(x => new
                {
                    x.OfficeId,
                    x.OfficeMail,
                    x.OfficeContactPersonName,
                    x.OfficeContactNumber
                });
        }

        public async Task<object> GetAgencySummaryAsync(int agencyId)
        {
            var offices = await _repository.GetAgencyOfficesAsync(agencyId);

            return new
            {
                AgencyId = agencyId,
                TotalOffices = offices.Count()
            };
        }

        public async Task<IEnumerable<object>> GetOfficeBookingsAsync(
            int agencyId,
            int officeId)
        {
            return (await _repository.GetOfficeBookingsAsync(
                agencyId,
                officeId))
                .Select(x => new
                {
                    x.BookingId,
                    x.SeatNumber,
                    x.Status
                });
        }

        public async Task<IEnumerable<object>> GetOfficePaymentsAsync(
            int agencyId,
            int officeId)
        {
            return (await _repository.GetOfficePaymentsAsync(
                agencyId,
                officeId))
                .Select(x => new
                {
                    x.PaymentId,
                    x.Amount,
                    x.PaymentStatus
                });
        }

        public async Task<IEnumerable<object>> GetOfficeTripsAsync(
            int agencyId,
            int officeId)
        {
            return (await _repository.GetOfficeTripsAsync(
                agencyId,
                officeId))
                .Select(x => new
                {
                    x.TripId,
                    x.DepartureTime,
                    x.ArrivalTime,
                    x.Fare
                });
        }

        public async Task<IEnumerable<object>> GetOfficeBusesAsync(
            int agencyId,
            int officeId)
        {
            return (await _repository.GetOfficeBusesAsync(
                agencyId,
                officeId))
                .Select(x => new
                {
                    x.BusId,
                    x.RegistrationNumber,
                    x.Capacity,
                    x.Type
                });
        }

        public async Task<IEnumerable<object>> GetOfficeDriversAsync(
            int agencyId,
            int officeId)
        {
            return (await _repository.GetOfficeDriversAsync(
                agencyId,
                officeId))
                .Select(x => new
                {
                    x.DriverId,
                    x.Name,
                    x.Phone,
                    x.LicenseNumber
                });
        }
    }
}
