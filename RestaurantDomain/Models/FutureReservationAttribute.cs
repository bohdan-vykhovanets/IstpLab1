using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantDomain.Models.Attributes
{
    public class FutureReservationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (!(value is DateTime selected))
            {
                return new ValidationResult("Некоректне значення дати та часу");
            }

            DateTime now = DateTime.Now;

            DateTime minAllowed = now.AddHours(12);

            if (minAllowed.Hour < 10)
            {
                minAllowed = new DateTime(minAllowed.Year, minAllowed.Month, minAllowed.Day, 10, 0, 0);
            }
            else if (minAllowed.Hour >= 22)
            {
                minAllowed = new DateTime(minAllowed.Year, minAllowed.Month, minAllowed.Day, 10, 0, 0).AddDays(1);
            }

            if (selected < minAllowed)
            {
                return new ValidationResult(
                    $"Вибраний час повинен бути хоча б {minAllowed.ToShortTimeString()} {minAllowed.ToShortDateString()} (приблизно через {(minAllowed - now).TotalHours:F1} годин)."
                );
            }

            if (selected.Hour < 10 || selected.Hour >= 22)
            {
                return new ValidationResult("Час повинен бути між 10 та 22 годинами.");
            }

            return ValidationResult.Success;
        }
    }
}