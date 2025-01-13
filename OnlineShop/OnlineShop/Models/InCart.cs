using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class InCart
    {
        [Key] // Add the Key attribute to designate the primary key
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; } // You can still keep this if you want to associate items with users, but you don’t have to use it.
    }
}
