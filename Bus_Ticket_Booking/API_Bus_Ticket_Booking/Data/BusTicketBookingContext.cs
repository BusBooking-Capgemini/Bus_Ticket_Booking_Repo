using System;
using System.Collections.Generic;
using API_Bus_Ticket_Booking.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Bus_Ticket_Booking.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Agency> Agencies { get; set; }

    public virtual DbSet<AgencyOffice> AgencyOffices { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Bus> Buses { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<API_Bus_Ticket_Booking.Models.Route> Routes { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-1P9GQUC\\SQLEXPRESS;Database=busticketbooking;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__addresse__CAA247C8AC3401C6");
        });

        modelBuilder.Entity<Agency>(entity =>
        {
            entity.HasKey(e => e.AgencyId).HasName("PK__agencies__7224EBF866BEB10E");
        });

        modelBuilder.Entity<AgencyOffice>(entity =>
        {
            entity.HasKey(e => e.OfficeId).HasName("PK__agency_o__2A1963753D7A8EEB");

            entity.Property(e => e.OfficeContactNumber).IsFixedLength();

            entity.HasOne(d => d.Agency).WithMany(p => p.AgencyOffices).HasConstraintName("FK_agency_offices_agency");

            entity.HasOne(d => d.OfficeAddress).WithMany(p => p.AgencyOffices).HasConstraintName("FK_agency_offices_address");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__bookings__5DE3A5B1EF403249");

            entity.HasOne(d => d.Trip).WithMany(p => p.Bookings).HasConstraintName("FK_bookings_trip");
        });

        modelBuilder.Entity<Bus>(entity =>
        {
            entity.HasKey(e => e.BusId).HasName("PK__buses__6ACEF8ED4EB24C8A");

            entity.HasOne(d => d.Office).WithMany(p => p.Buses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_buses_office");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__customer__CD65CB85BDCF953E");

            entity.HasOne(d => d.Address).WithMany(p => p.Customers).HasConstraintName("FK_customers_address");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.DriverId).HasName("PK__drivers__A411C5BDC00BDCA8");

            entity.HasOne(d => d.Address).WithMany(p => p.Drivers).HasConstraintName("FK_drivers_address");

            entity.HasOne(d => d.Office).WithMany(p => p.Drivers).HasConstraintName("FK_drivers_office");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__payments__ED1FC9EA4615804F");

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_payments_booking");

            entity.HasOne(d => d.Customer).WithMany(p => p.Payments).HasConstraintName("FK_payments_customer");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__reviews__60883D90479929DA");

            entity.Property(e => e.ReviewId).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithMany(p => p.Reviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_reviews_customer");

            entity.HasOne(d => d.Trip).WithMany(p => p.Reviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_reviews_trip");
        });

        modelBuilder.Entity<API_Bus_Ticket_Booking.Models.Route>(entity =>
        {
            entity.HasKey(e => e.RouteId).HasName("PK__routes__28F706FE765B6013");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.TripId).HasName("PK__trips__302A5D9EBE117BAB");

            entity.HasOne(d => d.Bus).WithMany(p => p.Trips)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_trips_bus");

            entity.HasOne(d => d.Driver1Driver).WithMany(p => p.TripDriver1Drivers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_trips_driver1");

            entity.HasOne(d => d.Driver2Driver).WithMany(p => p.TripDriver2Drivers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_trips_driver2");

            entity.HasOne(d => d.Route).WithMany(p => p.Trips)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_trips_route");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
