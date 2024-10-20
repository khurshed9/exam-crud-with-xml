namespace examWithXML.Entities;

public class GetProductCategorySupplier
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string SupplierName { get; set; } = null!;

    public int TotalOrders { get; set; }
}