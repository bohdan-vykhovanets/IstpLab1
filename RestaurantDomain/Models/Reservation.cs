using System;
using System.Collections.Generic;

namespace RestaurantInfrastructure;

public partial class Reservation
{
    public int ReservationId { get; set; }

    public int? UserId { get; set; }

    public DateTime? ReservationDate { get; set; }

    public int? NumberOfGuests { get; set; }

    public string? SpecialRequests { get; set; }

    public virtual ICollection<PreOrder> PreOrders { get; set; } = new List<PreOrder>();

    public virtual ICollection<Table> Tables { get; set; } = new List<Table>();

    public virtual User? User { get; set; }
}
