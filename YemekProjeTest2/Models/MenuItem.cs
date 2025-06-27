using System;
using System.Collections.Generic;

namespace YemekProjeTest2.Models;

public partial class MenuItem
{
    public int MenuItemId { get; set; }

    public int? RestaurantId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public double Price { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Restaurant? Restaurant { get; set; }
}
