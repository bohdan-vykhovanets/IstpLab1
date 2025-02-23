using System;
using System.Collections.Generic;

namespace RestaurantInfrastructure;

public partial class Cousine
{
    public int CousineId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
