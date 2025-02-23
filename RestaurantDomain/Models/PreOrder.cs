using System;
using System.Collections.Generic;

namespace RestaurantInfrastructure;

public partial class PreOrder
{
    public int PreOrderId { get; set; }

    public int? ReservationId { get; set; }

    public int? MenuItemId { get; set; }

    public int? Quantity { get; set; }

    public virtual MenuItem? MenuItem { get; set; }

    public virtual Reservation? Reservation { get; set; }
}
