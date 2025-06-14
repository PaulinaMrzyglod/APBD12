﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models;

[Table("Trip_DBFirst")]
public partial class TripDbfirst
{
    [Key]
    public int IdTrip { get; set; }

    [StringLength(120)]
    public string Name { get; set; } = null!;

    [StringLength(220)]
    public string Description { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime DateFrom { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateTo { get; set; }

    public int MaxPeople { get; set; }

    [InverseProperty("IdTripNavigation")]
    public virtual ICollection<ClientTripDbfirst> ClientTripDbfirsts { get; set; } = new List<ClientTripDbfirst>();

    [ForeignKey("IdTrip")]
    [InverseProperty("IdTrips")]
    public virtual ICollection<CountryDbfirst> IdCountries { get; set; } = new List<CountryDbfirst>();
}
