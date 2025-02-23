using System;
using System.Collections.Generic;

namespace RestaurantInfrastructure;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
