using System;
using System.Collections.Generic;

namespace RestaurantInfrastructure;

public partial class MenuItem
{
    public int MenuItemId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? ImageUrl { get; set; }

    public bool? Availability { get; set; }

    public virtual ICollection<PreOrder> PreOrders { get; set; } = new List<PreOrder>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Cousine> Cousines { get; set; } = new List<Cousine>();
}
