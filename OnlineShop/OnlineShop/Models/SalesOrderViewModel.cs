namespace OnlineShop.Models
{
    public class SalesOrderViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; } // If you still want to include the UserId for each order, keep it here.
        public decimal LineTotal { get; set; }  // Optional, to show the line total for this product
        public decimal TotalDue { get; set; }   // Optional, for the overall total for this product
    }
}