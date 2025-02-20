namespace GameHive.Models.Shopping_Cart_View_Models
{
    public class ShoppingCartRemoveViewModel
    {
        public string Message { get; set; }
        public decimal CartTotal { get; set; }
        public int CartCount { get; set; }
        public int DeleteId { get; set; }
    }
}
