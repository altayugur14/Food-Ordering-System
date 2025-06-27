using System;
using System.Collections.Generic;

namespace YemekProjeTest2.Models;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public int? UserId { get; set; }

    public int? MenuItemId { get; set; }

    public int Quantity { get; set; }

    public virtual MenuItem? MenuItem { get; set; }

    public virtual User? User { get; set; }
}
