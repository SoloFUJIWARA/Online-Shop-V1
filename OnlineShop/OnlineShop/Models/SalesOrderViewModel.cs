namespace OnlineShop.Models;

public class SalesOrderViewModel
{
    public int SalesOrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string SalesOrderNumber { get; set; }
    public byte Status { get; set; }
    public int ProductId { get; set; }
    public short OrderQty { get; set; }
    public decimal LineTotal { get; set; }
    public decimal TotalDue { get; set; }
}
