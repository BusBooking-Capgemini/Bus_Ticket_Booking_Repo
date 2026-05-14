using API_Bus_Ticket_Booking.DTOs.Customer;
using API_Bus_Ticket_Booking.DTOs.Review;

namespace TEST_Bus_Ticket_Booking.TestData
{
    public static class CustomerTestData
    {
        public static CustomerRequestDto GetCustomerRequestDto()
        {
            return new CustomerRequestDto
            {
                Name = "Aarav Sharma",
                Email = "aarav@test.com",
                Phone = "9876543210",
                Address = "Street 1",
                City = "Delhi",
                State = "Delhi",
                ZipCode = "110001"
            };
        }

        public static CustomerResponseDto GetCustomerResponseDto()
        {
            return new CustomerResponseDto
            {
                CustomerId = 1,
                Name = "Aarav Sharma",
                Email = "aarav@test.com",
                Phone = "9876543210",
                Address = "Street 1",
                City = "Delhi",
                State = "Delhi",
                ZipCode = "110001"
            };
        }

        public static ReviewRequestDto GetReviewRequestDto()
        {
            return new ReviewRequestDto
            {
                TripId = 1,
                Rating = 5,
                Comment = "Great trip"
            };
        }

        public static ReviewResponseDto GetReviewResponseDto()
        {
            return new ReviewResponseDto
            {
                ReviewId = 1,
                CustomerId = 1,
                CustomerName = "Aarav Sharma",
                TripId = 1,
                FromCity = "Delhi",
                ToCity = "Mumbai",
                Rating = 5,
                Comment = "Great trip",
                ReviewDate = new DateTime(2026, 5, 14)
            };
        }

        public static List<ReviewResponseDto> GetReviewResponses()
        {
            return new List<ReviewResponseDto>
            {
                GetReviewResponseDto(),
                new ReviewResponseDto
                {
                    ReviewId = 2,
                    CustomerId = 1,
                    CustomerName = "Aarav Sharma",
                    TripId = 2,
                    FromCity = "Delhi",
                    ToCity = "Jaipur",
                    Rating = 4,
                    Comment = "Comfortable journey",
                    ReviewDate = new DateTime(2026, 5, 13)
                }
            };
        }
    }
}