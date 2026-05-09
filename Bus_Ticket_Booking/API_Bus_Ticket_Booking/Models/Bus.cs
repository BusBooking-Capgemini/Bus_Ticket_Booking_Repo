using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Models;

[Table("buses")]
public partial class Bus
{
    [Key]
    [Column("bus_id")]
    public int BusId { get; set; }

    [Column("office_id")]
    public int OfficeId { get; set; }

    [Column("registration_number")]
    [StringLength(20)]
    [Unicode(false)]
    public string RegistrationNumber { get; set; } = null!;

    [Column("capacity")]
    public int Capacity { get; set; }

    [Column("type")]
    [StringLength(30)]
    [Unicode(false)]
    public string Type { get; set; } = null!;

    [ForeignKey("OfficeId")]
    [InverseProperty("Buses")]
    public virtual AgencyOffice Office { get; set; } = null!;

    [InverseProperty("Bus")]
    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
