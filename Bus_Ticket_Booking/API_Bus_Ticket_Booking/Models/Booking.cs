using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Models;

[Table("bookings")]
public partial class Booking
{
    [Key]
    [Column("booking_id")]
    public int BookingId { get; set; }

    [Column("trip_id")]
    public int? TripId { get; set; }

    [Column("seat_number")]
    public int SeatNumber { get; set; }

    [Column("status")]
    [StringLength(10)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [InverseProperty("Booking")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [ForeignKey("TripId")]
    [InverseProperty("Bookings")]
    public virtual Trip? Trip { get; set; }
}
