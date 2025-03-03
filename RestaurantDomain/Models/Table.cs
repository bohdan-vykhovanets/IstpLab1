using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RestaurantDomain.Models;

public partial class Table : Entity
{
    public int? ReservationId { get; set; }

    [DisplayName("Кількість місць")]
    [Required(ErrorMessage = "Столик повинен мати визначену кількість місць.")]
    [Range(1, 12, ErrorMessage = "У стола може бути від 1 до 12 місць.")]
    public int? NumberOfSeats { get; set; }

    [DisplayName("Доступність")]
    [Required(ErrorMessage = "Столик повинен мати визначену доступність.")]
    public bool? Availability { get; set; }

    [DisplayName("На вулиці")]
    [Required(ErrorMessage = "Столик повинен мати визначене розташування.")]
    public bool? IsOutdoor { get; set; }

    public virtual Reservation? Reservation { get; set; }
}
