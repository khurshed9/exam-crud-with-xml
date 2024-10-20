namespace examWithXML.Entities.Queries;

public class GetCategoryWithProd
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public int TotalAmountProduct { get; set; }
}