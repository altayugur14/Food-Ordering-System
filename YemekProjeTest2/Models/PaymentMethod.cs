using System;
using System.Collections.Generic;

namespace YemekProjeTest2.Models;

public partial class PaymentMethod
{
    public int PaymentMethodId { get; set; }

    public int? UserId { get; set; }

    public string CardName { get; set; } = null!;

    public string CardNumber { get; set; } = null!;

    public DateTime ExpiryDate { get; set; }

    public virtual User? User { get; set; }
}
