using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models;

[PrimaryKey("IdClient", "IdTrip")]
[Table("Client_Trip_DBFirst")]
public partial class ClientTripDbfirst
{
    [Key]
    public int IdClient { get; set; }

    [Key]
    public int IdTrip { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RegisteredAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PaymentDate { get; set; }

    [ForeignKey("IdClient")]
    [InverseProperty("ClientTripDbfirsts")]
    public virtual ClientDbfirst IdClientNavigation { get; set; } = null!;

    [ForeignKey("IdTrip")]
    [InverseProperty("ClientTripDbfirsts")]
    public virtual TripDbfirst IdTripNavigation { get; set; } = null!;
}
