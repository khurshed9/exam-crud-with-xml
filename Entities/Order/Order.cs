namespace examWithXML.Entities;

public class Order
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public DateTime OrderDate { get; set; }

    public string Status { get; set; } = null!;

    public int ProductId { get; set; }

    public int SupplierId { get; set; }
}