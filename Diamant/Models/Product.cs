using System;
using System.Collections.Generic;

namespace Diamant.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string NameProduct { get; set; } = null!;

    public decimal AssessedValue { get; set; }

    public decimal BailAmount { get; set; }

    public DateOnly DueDate { get; set; }

    public DateOnly ShelfLife { get; set; }

    public string StatusProduct { get; set; } = null!;

    public int ClientId { get; set; }

    public int EmployeeId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
