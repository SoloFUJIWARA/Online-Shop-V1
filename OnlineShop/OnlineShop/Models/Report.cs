namespace OnlineShop.Models;

public class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public decimal Amount { get; set; }
    public decimal Profit { get; set; }
    public DateTime OrderDate { get; set; }
    public string City { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual Product Product { get; set; }
    /*
    public virtual Category Category { get; set; }
*/
}
