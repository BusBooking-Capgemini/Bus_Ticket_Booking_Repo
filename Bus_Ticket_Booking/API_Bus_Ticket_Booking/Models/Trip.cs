using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Models;

[Table("trips")]
public partial class Trip
{
    [Key]
    [Column("trip_id")]
    public int TripId { get; set; }

    [Column("route_id")]
    public int RouteId { get; set; }

    [Column("bus_id")]
    public int BusId { get; set; }

    [Column("boarding_address_id")]
    public int BoardingAddressId { get; set; }

    [Column("dropping_address_id")]
    public int DroppingAddressId { get; set; }

    [Column("departure_time", TypeName = "datetime")]
    public DateTime DepartureTime { get; set; }

    [Column("arrival_time", TypeName = "datetime")]
    public DateTime ArrivalTime { get; set; }

    [Column("driver1_driver_id")]
    public int Driver1DriverId { get; set; }

    [Column("driver2_driver_id")]
    public int Driver2DriverId { get; set; }

    [Column("available_seats")]
    public int AvailableSeats { get; set; }

    [Column("fare", TypeName = "decimal(10, 2)")]
    public decimal Fare { get; set; }

    [Column("trip_date", TypeName = "datetime")]
    public DateTime TripDate { get; set; }

    [InverseProperty("Trip")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [ForeignKey("BusId")]
    [InverseProperty("Trips")]
    public virtual Bus Bus { get; set; } = null!;

    [ForeignKey("Driver1DriverId")]
    [InverseProperty("TripDriver1Drivers")]
    public virtual Driver Driver1Driver { get; set; } = null!;

    [ForeignKey("Driver2DriverId")]
    [InverseProperty("TripDriver2Drivers")]
    public virtual Driver Driver2Driver { get; set; } = null!;

    [InverseProperty("Trip")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [ForeignKey("RouteId")]
    [InverseProperty("Trips")]
    public virtual Route Route { get; set; } = null!;
}
