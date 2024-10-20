﻿namespace examWithXML.Entities;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public int CategoryId { get; set; }
}