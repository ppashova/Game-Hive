using System.ComponentModel.DataAnnotations;

namespace GameHive.Models.Order_View_Models
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Първото име е задължително")]
        [Display(Name = "Първо Име")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Последното име е задължително")]
        [Display(Name = "Последно име")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Емайл адресът е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден емайл адрес")]
        [Display(Name = "Емайл")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Името на картата е задължително")]
        [Display(Name = "Име на картата")]
        public string CardName { get; set; }

        [Required(ErrorMessage = "Номерът на картата е задължителен")]
        [RegularExpression(@"^[0-9\s]{13,19}$", ErrorMessage = "Невалиден номер на карта")]
        [Display(Name = "Номер на картата")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Месецът на валидност е задължителен")]
        [Display(Name = "Месец на валидност")]
        public string ExpiryMonth { get; set; }

        [Required(ErrorMessage = "Годината на валидност е задължителна")]
        [Display(Name = "Година на валидност")]
        public string ExpiryYear { get; set; }

        [Required(ErrorMessage = "CVV/CVC кодът е задължителен")]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "Невалиден CVV/CVC код")]
        [Display(Name = "CVV/CVC код")]
        public string Cvv { get; set; }

        [Display(Name = "Обща сума")]
        public decimal TotalPrice { get; set; }
        public List<Cart> CartItems { get; set; }

        public string GetExpirationDate()
        {
            if (!string.IsNullOrEmpty(ExpiryMonth) && !string.IsNullOrEmpty(ExpiryYear))
            {
                return $"{ExpiryMonth}/{ExpiryYear.Substring(2, 2)}";
            }
            return null;
        }
        public string GetMaskedCardNumber()
        {
            if (!string.IsNullOrEmpty(CardNumber))
            {
                string cleanNumber = CardNumber.Replace(" ", "");
                if (cleanNumber.Length >= 4)
                {
                    return $"**** **** **** {cleanNumber.Substring(cleanNumber.Length - 4)}";
                }
            }
            return null;
        }
    }

}
