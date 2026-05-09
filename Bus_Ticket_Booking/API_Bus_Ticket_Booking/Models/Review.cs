using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Models;

[Table("reviews")]
public partial class Review
{
    [Key]
    [Column("review_id")]
    public int ReviewId { get; set; }

    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Column("trip_id")]
    public int TripId { get; set; }

    [Column("rating")]
    public int Rating { get; set; }

    [Column("comment")]
    [Unicode(false)]
    public string? Comment { get; set; }

    [Column("review_date", TypeName = "datetime")]
    public DateTime? ReviewDate { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Reviews")]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey("TripId")]
    [InverseProperty("Reviews")]
    public virtual Trip Trip { get; set; } = null!;
}
