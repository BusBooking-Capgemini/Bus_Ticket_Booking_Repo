using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Models;

[Table("routes")]
public partial class Route
{
    [Key]
    [Column("route_id")]
    public int RouteId { get; set; }

    [Column("from_city")]
    [StringLength(255)]
    [Unicode(false)]
    public string FromCity { get; set; } = null!;

    [Column("to_city")]
    [StringLength(255)]
    [Unicode(false)]
    public string ToCity { get; set; } = null!;

    [Column("break_points")]
    public int? BreakPoints { get; set; }

    [Column("duration")]
    public int? Duration { get; set; }

    [InverseProperty("Route")]
    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
