using System;
using System.Collections.Generic;

namespace YemekProjeTest2.Models;

public partial class Cuisine
{
    public int CuisineId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<RestaurantCuisine> RestaurantCuisines { get; set; } = new List<RestaurantCuisine>();
}
