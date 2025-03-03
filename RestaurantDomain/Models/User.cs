using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RestaurantDomain.Models;

public partial class User : Entity
{
    [DisplayName("Ім'я")]
    [Required(ErrorMessage = "Користувач повинен мати ім'я.")]
    [StringLength(30, ErrorMessage = "Ім'я не повинно бути довше 30 символів.")]
    public string? Name { get; set; }

    [DisplayName("Адреса ел.пошти")]
    [Required(ErrorMessage = "Користувач повинен мати ел. пошту.")]
    [EmailAddress(ErrorMessage = "Введіть коректну адресу ел.пошти.")]
    public string? Email { get; set; }

    [DisplayName("Номер телефону")]
    [Required(ErrorMessage = "Користувач повинен мати телефон.")]
    [Phone(ErrorMessage = "Введіть коректний номер телефону.")]
    public string? PhoneNumber { get; set; }

    [DisplayName("Роль")]
    [Required(ErrorMessage = "Користувач повинен мати визначену роль.")]
    public bool? IsAdmin { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
