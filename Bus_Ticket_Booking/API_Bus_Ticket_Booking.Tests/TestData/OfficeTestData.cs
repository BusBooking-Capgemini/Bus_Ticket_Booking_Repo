using API_Bus_Ticket_Booking.DTOs.Office;
using API_Bus_Ticket_Booking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST_Bus_Ticket_Booking.TestData
{
    public static class OfficeTestData
    {
        public static List<OfficeResponseDto> GetOfficeResponseDtos()
        {
            return new List<OfficeResponseDto>
            {
                new OfficeResponseDto
                {
                    OfficeId = 1,
                    AgencyId = 1,
                    OfficeMail = "office1@test.com",
                    OfficeContactPersonName = "Rohit",
                    OfficeContactNumber = "9876543210",
                    OfficeAddressId = 1
                },
                new OfficeResponseDto
                {
                    OfficeId = 2,
                    AgencyId = 1,
                    OfficeMail = "office2@test.com",
                    OfficeContactPersonName = "Nikhil",
                    OfficeContactNumber = "9876543211",
                    OfficeAddressId = 2
                }
            };
        }

        public static OfficeResponseDto GetSingleOfficeResponseDto()
        {
            return new OfficeResponseDto
            {
                OfficeId = 1,
                AgencyId = 1,
                OfficeMail = "office@test.com",
                OfficeContactPersonName = "Rohit",
                OfficeContactNumber = "9876543210",
                OfficeAddressId = 1
            };
        }

        public static OfficeRequestDto GetOfficeRequestDto()
        {
            return new OfficeRequestDto
            {
                AgencyId = 1,
                OfficeMail = "office@test.com",
                OfficeContactPersonName = "Rohit",
                OfficeContactNumber = "9876543210",
                OfficeAddressId = 1
            };
        }

        public static AgencyOffice GetAgencyOffice()
        {
            return new AgencyOffice
            {
                OfficeId = 1,
                AgencyId = 1,
                OfficeMail = "office@test.com",
                OfficeContactPersonName = "Rohit",
                OfficeContactNumber = "9876543210",
                OfficeAddressId = 1
            };
        }

        public static List<Bus> GetBuses()
        {
            return new List<Bus>
            {
                new Bus
                {
                    BusId = 1,
                    OfficeId = 1,
                    RegistrationNumber = "PB10AA1111",
                    Capacity = 40,
                    Type = "AC"
                },
                new Bus
                {
                    BusId = 2,
                    OfficeId = 1,
                    RegistrationNumber = "PB10AA2222",
                    Capacity = 45,
                    Type = "Non-AC"
                }
            };
        }

        public static List<Driver> GetDrivers()
        {
            return new List<Driver>
            {
                new Driver
                {
                    DriverId = 1,
                    OfficeId = 1,
                    Name = "Rohit",
                    Phone = "9876543210"
                },
                new Driver
                {
                    DriverId = 2,
                    OfficeId = 1,
                    Name = "Nikhil",
                    Phone = "9876543211"
                }
            };
        }

        public static List<object> GetPayments()
        {
            return new List<object>
            {
                new
                {
                    PaymentId = 1,
                    Amount = 1200
                },
                new
                {
                    PaymentId = 2,
                    Amount = 1500
                }
            };
        }

        public static List<object> GetTrips()
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
    }
}
