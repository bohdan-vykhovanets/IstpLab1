using System;
using System.Collections.Generic;

namespace RestaurantInfrastructure;

public partial class Table
{
    public int TableId { get; set; }

    public int? ReservationId { get; set; }

    public int? NumberOfSeats { get; set; }

    public bool? Availability { get; set; }

    public bool? IsOutdoor { get; set; }

    public virtual Reservation? Reservation { get; set; }
}
