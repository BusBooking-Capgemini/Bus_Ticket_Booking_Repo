using System;
using System.Collections.Generic;

namespace API_Bus_Ticket_Booking.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int? AddressId { get; set; }

    public Guid? RoleId { get; set; }

    public string? PasswordHash { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Role? Role { get; set; }
}
