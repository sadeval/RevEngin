using System;
using System.Collections.Generic;

namespace RevEngin.Models;

public partial class Dish
{
    public int DishId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }
}
