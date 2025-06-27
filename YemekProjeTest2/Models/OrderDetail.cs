using System;
using System.Collections.Generic;

namespace YemekProjeTest2.Models;

public partial class OrderDetail
{
    public int OrderDetailsId { get; set; }

    public int? OrderId { get; set; }

    public int? MenuItemId { get; set; }

    public int Quantity { get; set; }

    public virtual MenuItem? MenuItem { get; set; }

    public virtual Order? Order { get; set; }
}
