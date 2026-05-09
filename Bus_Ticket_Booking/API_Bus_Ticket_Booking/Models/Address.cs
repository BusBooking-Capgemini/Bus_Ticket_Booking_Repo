using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Models;

[Table("addresses")]
public partial class Address
{
    [Key]
    [Column("address_id")]
    public int AddressId { get; set; }

    [Column("address")]
    [StringLength(255)]
    [Unicode(false)]
    public string Address1 { get; set; } = null!;

    [Column("city")]
    [StringLength(255)]
    [Unicode(false)]
    public string City { get; set; } = null!;

    [Column("state")]
    [StringLength(255)]
    [Unicode(false)]
    public string State { get; set; } = null!;

    [Column("zip_code")]
    [StringLength(10)]
    [Unicode(false)]
    public string ZipCode { get; set; } = null!;

    [InverseProperty("OfficeAddress")]
    public virtual ICollection<AgencyOffice> AgencyOffices { get; set; } = new List<AgencyOffice>();

    [InverseProperty("Address")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    [InverseProperty("Address")]
    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();
}
