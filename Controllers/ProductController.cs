using examWithXML.Entities;
using examWithXML.Services.ProductService;
using Microsoft.AspNetCore.Mvc;

namespace examWithXML.Controllers;

[Route("/api/products")]
[ApiController]

public class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> GetProducts()
    {
        return Results.Ok(await productService.GetProductsAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetProductByIdAsync(int id)
    {
        if (id <= 0) return Results.BadRequest();
        var product = await productService.GetProductByIdAsync(id);
        if (product == null) return Results.NotFound();
        return Results.Ok(await productService.GetProductByIdAsync(id));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateProductAsync([FromBody] Product product)
    {
        if (product == null) return Results.BadRequest("Failed to create product.");
        await productService.CreateProductAsync(product);
        return Results.Ok("Product created successfully.");
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> UpdateProductAsync([FromBody] Product product)
    {
        if (product == null) return Results.BadRequest("Failed to update product.");
        if (await productService.GetProductByIdAsync(product.Id) == null) return Results.NotFound();
        await productService.UpdateProductAsync(product);
        return Results.Ok("Product updated successfully.");
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteProductAsync(int id)
    {
        if (await productService.DeleteProductAsync(id) == false) return Results.NotFound("Product with this ID doesn't exist");
        return Results.Ok(await productService.DeleteProductAsync(id));
    }

    [HttpGet("quantity/{quantity}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> GetProductsByQuantityAsync(int quantity)
    {
        return Results.Ok(await productService.GetProductByQuantityAsync(quantity));
    }

    [HttpGet("/minOrders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> GetProductsWithMinOrdersAsync()
    {
        return Results.Ok(await productService.GetProductOrdered5TimeMAsync());
    }

    [HttpGet("{productId}/details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> GetProductsByCategoryIdAsync(int productId)
    {
        return Results.Ok(await productService.GetProductCategorySupplierAsync(productId));
    }

    [HttpGet("categoryId={categoryId}/sortOrder={sortOrder}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> GetProductByCategoryAndPriceAsync(int categoryId, string sortOrder)
    {
        return Results.Ok(await productService.GetProductByCategoryAndPriceAsync(categoryId, sortOrder));
    }

    [HttpGet("supplierId={supplierId}/sortOrder={sortOrder}/include")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> GetProductBySupplierAndPriceAsync(int supplierId, int sortOrder)
    {
        return Results.Ok(await productService.GetProductsWithDetailsAsync(supplierId, sortOrder));
    }

}