﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RestaurantDomain.Models;

public partial class Category : Entity
{
    [DisplayName("Назва")]
    [Required(ErrorMessage = "У категорії повинна бути назва.")]
    [StringLength(50, ErrorMessage = "Назва не повинна бути довше 50 символів.")]
    public string? Name { get; set; }

    public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
