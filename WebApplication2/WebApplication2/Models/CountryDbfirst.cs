using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models;

[Table("Country_DBFirst")]
public partial class CountryDbfirst
{
    [Key]
    public int IdCountry { get; set; }

    [StringLength(120)]
    public string Name { get; set; } = null!;

    [ForeignKey("IdCountry")]
    [InverseProperty("IdCountries")]
    public virtual ICollection<TripDbfirst> IdTrips { get; set; } = new List<TripDbfirst>();
}
