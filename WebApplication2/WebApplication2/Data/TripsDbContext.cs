using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data;

public partial class TripsDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public TripsDbContext()
    {
    }

    public TripsDbContext(DbContextOptions<TripsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ClientDbfirst> ClientDbfirsts { get; set; }

    public virtual DbSet<ClientTripDbfirst> ClientTripDbfirsts { get; set; }

    public virtual DbSet<CountryDbfirst> CountryDbfirsts { get; set; }

    public virtual DbSet<TripDbfirst> TripDbfirsts { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       // => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Default");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("s31987");

        modelBuilder.Entity<ClientDbfirst>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("Client_DBFirst_pk");
        });

        modelBuilder.Entity<ClientTripDbfirst>(entity =>
        {
            entity.HasKey(e => new { e.IdClient, e.IdTrip }).HasName("Client_Trip_DBFirst_pk");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.ClientTripDbfirsts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Client_Trip_DBFirst_Client_DBFirst");

            entity.HasOne(d => d.IdTripNavigation).WithMany(p => p.ClientTripDbfirsts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Client_Trip_DBFirst_Trip_DBFirst");
        });

        modelBuilder.Entity<CountryDbfirst>(entity =>
        {
            entity.HasKey(e => e.IdCountry).HasName("Country_DBFirst_pk");

            entity.HasMany(d => d.IdTrips).WithMany(p => p.IdCountries)
                .UsingEntity<Dictionary<string, object>>(
                    "CountryTripDbfirst",
                    r => r.HasOne<TripDbfirst>().WithMany()
                        .HasForeignKey("IdTrip")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Country_Trip_DBFirst_Trip_DBFirst"),
                    l => l.HasOne<CountryDbfirst>().WithMany()
                        .HasForeignKey("IdCountry")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Country_Trip_DBFirst_Country_DBFirst"),
                    j =>
                    {
                        j.HasKey("IdCountry", "IdTrip").HasName("Country_Trip_DBFirst_pk");
                        j.ToTable("Country_Trip_DBFirst");
                    });
        });

        modelBuilder.Entity<TripDbfirst>(entity =>
        {
            entity.HasKey(e => e.IdTrip).HasName("Trip_DBFirst_pk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
