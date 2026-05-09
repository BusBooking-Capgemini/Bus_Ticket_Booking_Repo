using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Models;

[Table("agencies")]
public partial class Agency
{
    [Key]
    [Column("agency_id")]
    public int AgencyId { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("contact_person_name")]
    [StringLength(30)]
    [Unicode(false)]
    public string ContactPersonName { get; set; } = null!;

    [Column("email")]
    [StringLength(255)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("phone")]
    [StringLength(15)]
    [Unicode(false)]
    public string Phone { get; set; } = null!;

    [InverseProperty("Agency")]
    public virtual ICollection<AgencyOffice> AgencyOffices { get; set; } = new List<AgencyOffice>();
}
