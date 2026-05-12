using System;
using System.Collections.Generic;

namespace API_Bus_Ticket_Booking.Models;

public partial class Role
{
    public Guid RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Agency> Agencies { get; set; } = new List<Agency>();

    public virtual ICollection<AgencyOffice> AgencyOffices { get; set; } = new List<AgencyOffice>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
