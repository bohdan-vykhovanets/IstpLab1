using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RestaurantInfrastructure.Models
{
    public class MenuItemCreateViewModel
    {
        [DisplayName("Назва")]
        [Required(ErrorMessage = "Позиція повинна мати ім'я.")]
        [StringLength(50, ErrorMessage = "Ім'я не повинно бути довше 50 символів.")]
        public string? Name { get; set; }

        [DisplayName("Опис")]
        [Required(ErrorMessage = "Позиція повинна мати опис.")]
        [StringLength(250, ErrorMessage = "Опис не повинен бути довше 250 символів.")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [DisplayName("Ціна")]
        [Required(ErrorMessage = "У позиції повинна бути ціна.")]
        [DataType(DataType.Currency, ErrorMessage = "Введіть коректне значення ціни.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ціна повинна бути додатньою.")]
        public decimal? Price { get; set; }

        [DisplayName("Зображення")]
        [DataType(DataType.Url, ErrorMessage = "Введіть коректне URL зображення.")]
        public string? ImageUrl { get; set; }

        [DisplayName("Наявність")]
        [Required(ErrorMessage = "Наявність повинна бути вказана.")]
        public bool? Availability { get; set; }

        public List<int> SelectedCategoryIds { get; set; } = new List<int>();
        public List<int> SelectedCousineIds { get; set; } = new List<int>();

        [BindNever, ValidateNever]
        public IEnumerable<SelectListItem> AvailableCategories { get; set; }
        [BindNever, ValidateNever]
        public IEnumerable<SelectListItem> AvailableCousines { get; set; }
    }

}
