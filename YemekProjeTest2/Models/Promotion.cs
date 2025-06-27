using System;
using System.Collections.Generic;

namespace YemekProjeTest2.Models;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public int? RestaurantId { get; set; }

    public string PromoCode { get; set; } = null!;

    public double DiscountPercentage { get; set; }

    public virtual Restaurant? Restaurant { get; set; }
}
