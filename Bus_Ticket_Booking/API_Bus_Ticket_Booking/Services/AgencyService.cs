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

        public AgencyService(
            IAgencyRepository repository,
            IMapper mapper)
        {
            _repository = repository;

            _mapper = mapper;
        }

        // =========================
        // GET ALL AGENCIES
        // =========================

        public async Task<IEnumerable<AgencyResponseDto>>
            GetAllAsync()
        {
            var agencies =
                await _repository.GetAllAsync();

            if (!agencies.Any())
            {
                throw new NotFoundException(
                    "No agencies found");
            }

            return _mapper.Map<
                IEnumerable<AgencyResponseDto>>(
                    agencies);
        }

        // =========================
        // GET AGENCY BY ID
        // =========================

        public async Task<AgencyResponseDto>
            GetByIdAsync(int id)
        {
            var agency =
                await _repository.GetByIdAsync(id);

            if (agency == null)
            {
                throw new NotFoundException(
                    "Agency not found");
            }

            return _mapper.Map<
                AgencyResponseDto>(
                    agency);
        }

        // =========================
        // UPDATE AGENCY
        // =========================

        public async Task
            UpdateAsync(
                int id,
                AgencyRequestDto dto)
        {
            var agency =
                await _repository.GetByIdAsync(id);

            if (agency == null)
            {
                throw new NotFoundException(
                    "Agency not found");
            }

            agency.Name =
                dto.Name;

            agency.ContactPersonName =
                dto.ContactPersonName;

            agency.Email =
                dto.Email;

            agency.Phone =
                dto.Phone;

            await _repository
                .UpdateAsync(agency);
        }

        // =========================
        // DELETE AGENCY
        // =========================

        public async Task
            DeleteAsync(int id)
        {
            var agency =
                await _repository.GetByIdAsync(id);

            if (agency == null)
            {
                throw new NotFoundException(
                    "Agency not found");
            }

            await _repository
                .DeleteAsync(agency);
        }

        // =========================
        // GET AGENCY OFFICES
        // =========================

        public async Task<IEnumerable<object>>
            GetAgencyOfficesAsync(
                int agencyId)
        {
            var agency =
                await _repository
                    .GetByIdAsync(agencyId);

            if (agency == null)
            {
                throw new NotFoundException(
                    "Agency not found");
            }

            var offices =
                await _repository
                    .GetAgencyOfficesAsync(
                        agencyId);

            if (!offices.Any())
            {
                throw new NotFoundException(
                    "No offices found");
            }

            return offices.Select(x => new
            {
                x.OfficeId,

                x.OfficeMail,

                x.OfficeContactPersonName,

                x.OfficeContactNumber
            });
        }

        // =========================
        // AGENCY SUMMARY
        // =========================

        public async Task<object>
            GetAgencySummaryAsync(
                int agencyId)
        {
            var agency =
                await _repository
                    .GetByIdAsync(agencyId);

            if (agency == null)
            {
                throw new NotFoundException(
                    "Agency not found");
            }

            var offices =
                await _repository
                    .GetAgencyOfficesAsync(
                        agencyId);

            return new
            {
                AgencyId = agencyId,

                TotalOffices =
                    offices.Count()
            };
        }

        // =========================
        // OFFICE BOOKINGS
        // =========================

        public async Task<IEnumerable<object>>
            GetOfficeBookingsAsync(
                int agencyId,
                int officeId)
        {
            var bookings =
                await _repository
                    .GetOfficeBookingsAsync(
                        agencyId,
                        officeId);

            var filteredBookings =
                bookings
                    .Where(x =>
                        x.Status == "Booked");

            if (!filteredBookings.Any())
            {
                throw new NotFoundException(
                    "No bookings found");
            }

            return filteredBookings.Select(x => new
            {
                x.BookingId,

                TripId =
                    x.TripId,

                x.SeatNumber,

                x.Status
            });
        }

        // =========================
        // OFFICE PAYMENTS
        // =========================

        public async Task<IEnumerable<object>>
            GetOfficePaymentsAsync(
                int agencyId,
                int officeId)
        {
            var payments =
                await _repository
                    .GetOfficePaymentsAsync(
                        agencyId,
                        officeId);

            if (!payments.Any())
            {
                throw new NotFoundException(
                    "No payments found");
            }

            return payments.Select(x => new
            {
                x.PaymentId,

                BookingId =
                    x.BookingId,

                x.Amount,

                Status =
                    x.PaymentStatus
            });
        }

        // =========================
        // OFFICE TRIPS
        // =========================

        public async Task<IEnumerable<object>>
            GetOfficeTripsAsync(
                int agencyId,
                int officeId)
        {
            var trips =
                await _repository
                    .GetOfficeTripsAsync(
                        agencyId,
                        officeId);

            if (!trips.Any())
            {
                throw new NotFoundException(
                    "No trips found");
            }

            return trips.Select(x => new
            {
                x.TripId,

                Route =
                    x.Route.FromCity +
                    " → " +
                    x.Route.ToCity,

                x.TripDate,

                x.DepartureTime,

                x.ArrivalTime,

                x.Fare
            });
        }

        // =========================
        // OFFICE BUSES
        // =========================

        public async Task<IEnumerable<object>>
            GetOfficeBusesAsync(
                int agencyId,
                int officeId)
        {
            var buses =
                await _repository
                    .GetOfficeBusesAsync(
                        agencyId,
                        officeId);

            if (!buses.Any())
            {
                throw new NotFoundException(
                    "No buses found");
            }

            return buses.Select(x => new
            {
                x.BusId,

                x.RegistrationNumber,

                x.Capacity,

                x.Type
            });
        }

        // =========================
        // OFFICE DRIVERS
        // =========================

        public async Task<IEnumerable<object>>
            GetOfficeDriversAsync(
                int agencyId,
                int officeId)
        {
            var drivers =
                await _repository
                    .GetOfficeDriversAsync(
                        agencyId,
                        officeId);

            if (!drivers.Any())
            {
                throw new NotFoundException(
                    "No drivers found");
            }

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