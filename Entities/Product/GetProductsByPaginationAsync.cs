namespace examWithXML.Entities;

public class GetProductsByPaginationAsync
{
    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string SupplierName { get; set; } = null!;

    public decimal Price { get; set; }
}