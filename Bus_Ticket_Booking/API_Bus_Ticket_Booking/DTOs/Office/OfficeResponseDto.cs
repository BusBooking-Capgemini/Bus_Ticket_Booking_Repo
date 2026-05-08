namespace API_Bus_Ticket_Booking.DTOs.Office
{
    public class OfficeResponseDto
    {
        public int OfficeId { get; set; }
        public int AgencyId { get; set; }
        public string OfficeMail { get; set; }
        public string OfficeContactPersonName { get; set; }
        public string OfficeContactNumber { get; set; }
        public int OfficeAddressId { get; set; }
    }
}
