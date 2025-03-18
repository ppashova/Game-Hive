using System.ComponentModel.DataAnnotations;

namespace GameHive.Models.Order_View_Models
{
    public class CheckoutViewModel
    {
        public List<Cart> CartItems { get; set; } = new List<Cart>();
        public decimal TotalPrice { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }

}
