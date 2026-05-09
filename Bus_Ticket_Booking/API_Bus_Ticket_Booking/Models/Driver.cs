using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Models;

[Table("drivers")]
public partial class Driver
{
    [Key]
    [Column("driver_id")]
    public int DriverId { get; set; }

    [Column("license_number")]
    [StringLength(20)]
    [Unicode(false)]
    public string LicenseNumber { get; set; } = null!;

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("phone")]
    [StringLength(15)]
    [Unicode(false)]
    public string Phone { get; set; } = null!;

    [Column("office_id")]
    public int? OfficeId { get; set; }

    [Column("address_id")]
    public int? AddressId { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("Drivers")]
    public virtual Address? Address { get; set; }

    [ForeignKey("OfficeId")]
    [InverseProperty("Drivers")]
    public virtual AgencyOffice? Office { get; set; }

    [InverseProperty("Driver1Driver")]
    public virtual ICollection<Trip> TripDriver1Drivers { get; set; } = new List<Trip>();

    [InverseProperty("Driver2Driver")]
    public virtual ICollection<Trip> TripDriver2Drivers { get; set; } = new List<Trip>();
}
