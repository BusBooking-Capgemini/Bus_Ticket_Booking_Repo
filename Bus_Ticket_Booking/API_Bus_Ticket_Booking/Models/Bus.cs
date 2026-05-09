using System;
using System.Collections.Generic;

namespace API_Bus_Ticket_Booking.Models;

public partial class Bus
{
    public int BusId { get; set; }

    public int OfficeId { get; set; }

    public string RegistrationNumber { get; set; } = null!;

    public int Capacity { get; set; }

    public string Type { get; set; } = null!;

    public virtual AgencyOffice Office { get; set; } = null!;

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
