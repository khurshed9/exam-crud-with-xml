namespace examWithXML.Entities;

public class GetProductOrdered5TimeM
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public int TotalOrdered { get; set; }
}