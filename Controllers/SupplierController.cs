using examWithXML.Entities;
using examWithXML.Services.SupplierService;
using Microsoft.AspNetCore.Mvc;

namespace examWithXML.Controllers;

[Route("api/suppliers")]
[ApiController]
public class SupplierController(ISupplierService supplierService) : ControllerBase
{

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> GetSuppliers()
    {
        return Results.Ok(await supplierService.GetSuppliersAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetSupplierByIdAsync(int id)
    {
        if (id <= 0) return Results.BadRequest();
        var supplier = await supplierService.GetSupplierByIdAsync(id);
        if (supplier == null) return Results.NotFound();
        return Results.Ok(supplier);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateSupplierAsync([FromBody] Supplier supplier)
    {
        if (supplier == null) return Results.BadRequest("Failed to create supplier.");
        await supplierService.CreateSupplierAsync(supplier);
        return Results.Created($"/api/suppliers/{supplier.Id}", "Supplier created successfully.");
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> UpdateSupplierAsync([FromBody] Supplier supplier)
    {
        if (supplier == null) return Results.BadRequest("Failed to update supplier.");
        if (await supplierService.GetSupplierByIdAsync(supplier.Id) == null) return Results.NotFound();
        await supplierService.UpdateSupplierAsync(supplier);
        return Results.Ok("Supplier updated successfully.");
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteSupplierAsync(int id)
    {
        if (await supplierService.DeleteSupplierAsync(id) == false) return Results.NotFound("Supplier with this ID doesn't exist.");
        return Results.Ok("Supplier deleted successfully.");
    }

    [HttpGet("/minProductQuantity={productQuantity}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> GetSupplierByProductQuantityAsync(int productQuantity)
    {
        return Results.Ok(await supplierService.GetSupplierByProductQuantityAsync(productQuantity));
    }
}
