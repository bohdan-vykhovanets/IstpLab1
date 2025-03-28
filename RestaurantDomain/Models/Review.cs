﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantDomain.Models;

public partial class Review : Entity
{
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; }
    
    [DisplayName("Оцінка")]
    [Required(ErrorMessage = "Оцінка повинна бути вказана.")]
    [Range(1, 5, ErrorMessage = "Оцінка може бути від 1 до 5.")]
    public int? Rating { get; set; }

    [DisplayName("Коментар")]
    [StringLength(500, ErrorMessage = "Довжина не повинна перевищувати 500 символів.")]
    public string? Comment { get; set; }

    [DisplayName("Залишений")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
}
