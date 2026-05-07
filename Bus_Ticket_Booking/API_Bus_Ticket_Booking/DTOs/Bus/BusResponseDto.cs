public class BusResponseDto
{
    public int BusId { get; set; }
    public int OfficeId { get; set; }
    public string OfficeName { get; set; } = null!;
    public string RegistrationNumber { get; set; } = null!;
    public int Capacity { get; set; }
    public string Type { get; set; } = null!;
}