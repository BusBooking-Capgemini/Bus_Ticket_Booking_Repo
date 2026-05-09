using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Models;

[Table("agency_offices")]
public partial class AgencyOffice
{
    [Key]
    [Column("office_id")]
    public int OfficeId { get; set; }

    [Column("agency_id")]
    public int? AgencyId { get; set; }

    [Column("office_mail")]
    [StringLength(100)]
    [Unicode(false)]
    public string? OfficeMail { get; set; }

    [Column("office_contact_person_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string? OfficeContactPersonName { get; set; }

    [Column("office_contact_number")]
    [StringLength(10)]
    [Unicode(false)]
    public string? OfficeContactNumber { get; set; }

    [Column("office_address_id")]
    public int? OfficeAddressId { get; set; }

    [ForeignKey("AgencyId")]
    [InverseProperty("AgencyOffices")]
    public virtual Agency? Agency { get; set; }

    [InverseProperty("Office")]
    public virtual ICollection<Bus> Buses { get; set; } = new List<Bus>();

    [InverseProperty("Office")]
    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();

    [ForeignKey("OfficeAddressId")]
    [InverseProperty("AgencyOffices")]
    public virtual Address? OfficeAddress { get; set; }
}
