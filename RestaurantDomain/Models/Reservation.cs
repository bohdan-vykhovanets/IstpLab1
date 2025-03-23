using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestaurantDomain.Models.Attributes;

namespace RestaurantDomain.Models;

public partial class Reservation : Entity
{
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    [DisplayName("Дата та час початку")]
    [Required(ErrorMessage = "Бронювання повинно мати дату та час.")]
    [DataType(DataType.DateTime)]
    [FutureReservation(ErrorMessage = "Бронювання повинно бути зроблене хоча б за 12 годин та в період між 10 та 22 годинами.")]
    public DateTime ReservationDateStart { get; set; }

    [DisplayName("Дата та час завершення")]
    [Required(ErrorMessage = "Бронювання повинно мати дату та час.")]
    [DataType(DataType.DateTime)]
    [FutureReservation(ErrorMessage = "Бронювання повинно бути зроблене хоча б за 12 годин та в період між 10 та 22 годинами.")]
    public DateTime ReservationDateEnd { get; set; }

    [DisplayName("Кількість гостей")]
    [Required(ErrorMessage = "Кількість гостей повинна бути вказана.")]
    [Range(1, 50, ErrorMessage = "Кількість гостей може бути від 1 до 50.")]
    public int? NumberOfGuests { get; set; }

    [DisplayName("Примітки та побажання")]
    [StringLength(1000, ErrorMessage = "Довжина не повинна перевищувати 1000 символів.")]
    public string? SpecialRequests { get; set; }

    public virtual ICollection<PreOrder> PreOrders { get; set; } = new List<PreOrder>();

    public virtual ICollection<Table> Tables { get; set; } = new List<Table>();
}
