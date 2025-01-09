namespace OnlineShop.ViewModels;

public class ProductCategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProductCount { get; set; }
    public DateTime ModifiedDate { get; set; }
    public bool CanDelete => ProductCount == 0;
}