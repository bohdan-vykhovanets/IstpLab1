using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RestaurantDomain.Models;

public partial class PreOrder : Entity
{
    [Required]
    public int? ReservationId { get; set; }

    [Required]
    public int? MenuItemId { get; set; }

    [DisplayName("Кількість")]
    [Required(ErrorMessage = "Кількість повинна бути вказана.")]
    [Range(1, int.MaxValue, ErrorMessage = "Кількість повинна бути додатньою.")]
    public int? Quantity { get; set; }

    public virtual MenuItem? MenuItem { get; set; }

    public virtual Reservation? Reservation { get; set; }
}
