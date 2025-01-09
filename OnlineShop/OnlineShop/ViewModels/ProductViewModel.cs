namespace OnlineShop.ViewModels;

public class ProductViewModel
{
    public int ProductId { get; set; }
    public string Name { get; set; } = null!;
    public string ProductNumber { get; set; } = null!;
    public string? Color { get; set; }
    public decimal ListPrice { get; set; }
    public int OrderCount { get; set; }
    public DateTime ModifiedDate { get; set; }
    public bool CanDelete => OrderCount == 0;

}