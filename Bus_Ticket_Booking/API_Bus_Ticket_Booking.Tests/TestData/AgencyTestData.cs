using API_Bus_Ticket_Booking.DTOs.Agency;
using API_Bus_Ticket_Booking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST_Bus_Ticket_Booking.TestData
{
    public static class AgencyTestData
    {
        public static List<AgencyResponseDto> GetAgencies()
        {
            return new List<AgencyResponseDto>
            {
                new AgencyResponseDto
                {
                    AgencyId = 1,
                    Name = "ABC Travels",
                    ContactPersonName = "Rohit",
                    Email = "abc@gmail.com",
                    Phone = "9876543210"
                },

                new AgencyResponseDto
                {
                    AgencyId = 2,
                    Name = "XYZ Travels",
                    ContactPersonName = "Nikhil",
                    Email = "xyz@gmail.com",
                    Phone = "9876543211"
                }
            };
        }

        public static AgencyResponseDto GetAgency()
        {
            return new AgencyResponseDto
            {
                AgencyId = 1,
                Name = "ABC Travels",
                ContactPersonName = "Rohit",
                Email = "abc@gmail.com",
                Phone = "9876543210"
            };
        }

        public static AgencyRequestDto GetAgencyRequestDto()
        {
            return new AgencyRequestDto
            {
                Name = "ABC Travels",
                ContactPersonName = "Rohit",
                Email = "abc@gmail.com",
                Phone = "9876543210"
            };
        }

        public static Agency GetAgencyEntity()
        {
            return new Agency
            {
                AgencyId = 1,
                Name = "ABC Travels",
                ContactPersonName = "Rohit",
                Email = "abc@gmail.com",
                Phone = "9876543210"
            };
        }

        public static List<AgencyOffice> GetAgencyOffices()
        {
            return new List<AgencyOffice>
            {
                new AgencyOffice
                {
                    OfficeId = 1,
                    AgencyId = 1,
                    OfficeMail = "office1@test.com",
                    OfficeContactPersonName = "Rohit",
                    OfficeContactNumber = "9876543210"
                },

                new AgencyOffice
                {
                    OfficeId = 2,
                    AgencyId = 1,
                    OfficeMail = "office2@test.com",
                    OfficeContactPersonName = "Nikhil",
                    OfficeContactNumber = "9876543211"
                }
            };
        }

        public static List<object> GetOfficeBookings()
        {
            return new List<object>
            {
                new
                {
                    BookingId = 1,
                    SeatNumber = 12,
                    Status = "Booked"
                },

                new
                {
                    BookingId = 2,
                    SeatNumber = 15,
                    Status = "Booked"
                }
            };
        }

        public static List<object> GetOfficePayments()
        {
            return new List<object>
            {
                new
                {
                    PaymentId = 1,
                    Amount = 1200,
                    PaymentStatus = "Paid"
                },

                new
                {
                    PaymentId = 2,
                    Amount = 1500,
                    PaymentStatus = "Paid"
                }
            };
        }

        public static List<object> GetOfficeTrips()
        {
            return new List<object>
            {
                new
                {
                    TripId = 1,
                    Fare = 800
                },

                new
                {
                    TripId = 2,
                    Fare = 1200
                }
            };
        }

        public static List<object> GetOfficeBuses()
        {
            return new List<object>
            {
                new
                {
                    BusId = 1,
                    RegistrationNumber = "PB10AA1111",
                    Capacity = 40,
                    Type = "AC"
                },

                new
                {
                    BusId = 2,
                    RegistrationNumber = "PB10AA2222",
                    Capacity = 45,
                    Type = "Non-AC"
                }
            };
        }

        public static List<object> GetOfficeDrivers()
        {
            return new List<object>
            {
                new
                {
                    DriverId = 1,
                    Name = "Rohit",
                    Phone = "9876543210",
                    LicenseNumber = "LIC123"
                },

                new
                {
                    DriverId = 2,
                    Name = "Nikhil",
                    Phone = "9876543211",
                    LicenseNumber = "LIC456"
                }
            };
        }

        public static object GetAgencySummary()
        {
            return new
            {
                AgencyId = 1,
                TotalOffices = 5
            };
        }
    }
}
