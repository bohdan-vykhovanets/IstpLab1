using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RestaurantInfrastructure.Models
{
    public class PreOrderCreateViewModel
    {
        [Required]
        public int? MenuItemId { get; set; }

        [DisplayName("Кількість")]
        [Required(ErrorMessage = "Кількість повинна бути вказана.")]
        [Range(1, int.MaxValue, ErrorMessage = "Кількість повинна бути додатньою.")]
        public int? Quantity { get; set; }
    }
}
