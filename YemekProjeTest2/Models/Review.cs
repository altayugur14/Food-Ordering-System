using System;
using System.Collections.Generic;

namespace YemekProjeTest2.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? UserId { get; set; }

    public int? RestaurantId { get; set; }

    public double Rating { get; set; }

    public string? Comment { get; set; }

    public int? OrderId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Restaurant? Restaurant { get; set; }

    public virtual User? User { get; set; }
}
