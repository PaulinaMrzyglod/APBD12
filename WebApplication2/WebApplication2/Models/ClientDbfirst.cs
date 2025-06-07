using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models;

[Table("Client_DBFirst")]
public partial class ClientDbfirst
{
    [Key]
    public int IdClient { get; set; }

    [StringLength(120)]
    public string FirstName { get; set; } = null!;

    [StringLength(120)]
    public string LastName { get; set; } = null!;

    [StringLength(120)]
    public string Email { get; set; } = null!;

    [StringLength(120)]
    public string Telephone { get; set; } = null!;

    [StringLength(120)]
    public string Pesel { get; set; } = null!;

    [InverseProperty("IdClientNavigation")]
    public virtual ICollection<ClientTripDbfirst> ClientTripDbfirsts { get; set; } = new List<ClientTripDbfirst>();
}
