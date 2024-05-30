using System;
using System.Collections.Generic;

namespace Diamant.Models;

public partial class Client
{
    public int ClientId { get; set; }

    public string LName { get; set; } = null!;

    public string FName { get; set; } = null!;

    public string? PName { get; set; }

    public DateOnly BDate { get; set; }

    public string Phone { get; set; } = null!;

    public string SPassport { get; set; } = null!;

    public string NPassport { get; set; } = null!;

    public string FullNameClient => $"{LName} {FName} {PName}"; 
    public string ShortNameClient => $"{LName} {FName[0]}. {PName?[0]}.";
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
