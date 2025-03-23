using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using RestaurantDomain.Models.Attributes;

namespace RestaurantInfrastructure.Models
{
    public class ReservationEditViewModel
    {
        public int ReservationId { get; set; }

        public string? UserId { get; set; }

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

        public List<int> SelectedTableIds { get; set; } = new List<int>();

        [BindNever, ValidateNever]
        public IEnumerable<SelectListItem> AvailableTables { get; set; }

        public List<PreOrderCreateViewModel> PreOrders { get; set; } = new List<PreOrderCreateViewModel>();

        [BindNever, ValidateNever]
        public IEnumerable<SelectListItem> AvailableMenuItems { get; set; }
    }
}
