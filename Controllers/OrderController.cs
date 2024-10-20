using examWithXML.Entities;
using examWithXML.Services.OrderService;
using Microsoft.AspNetCore.Mvc;

namespace examWithXML.Controllers;

[Route("api/orders")]
[ApiController]
public class OrderController(IOrderService orderService) : ControllerBase
{

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> GetOrders()
    {
        return Results.Ok(await orderService.GetOrdersAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetOrderByIdAsync(int id)
    {
        if (id <= 0) return Results.BadRequest();
        var order = await orderService.GetOrderByIdAsync(id);
        if (order == null) return Results.NotFound();
        return Results.Ok(order);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateOrderAsync([FromBody] Order order)
    {
        if (order == null) return Results.BadRequest("Failed to create order.");
        await orderService.CreateOrderAsync(order);
        return Results.Created($"/api/orders/{order.Id}", "Order created successfully.");
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> UpdateOrderAsync([FromBody] Order order)
    {
        if (order == null) return Results.BadRequest("Failed to update order.");
        if (await orderService.GetOrderByIdAsync(order.Id) == null) return Results.NotFound();
        await orderService.UpdateOrderAsync(order);
        return Results.Ok("Order updated successfully.");
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteOrderAsync(int id)
    {
        if (await orderService.DeleteOrderAsync(id) == false) return Results.NotFound("Order with this ID doesn't exist");
        return Results.Ok("Order deleted successfully.");
    }
    
    [HttpGet("/startDate={startDate}&endDate={endDate}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> GetOrderByOrderDateAsync(DateTime startDate, DateTime endDate)
    {
        return Results.Ok(await orderService.GetOrderByOrderDateAsync(startDate, endDate));
    }

    [HttpGet("/supplierId={supplierId}/status={status}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> GetOrderByProductIdAsync(int supplierId, string status)
    {
        return Results.Ok(await orderService.GetOrdersBySupplierAndStatusAsync(supplierId, status));
    }

    [HttpGet("/pageNumber={pageNumber}&pageSize={pageSize}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> GetOrdersByPaginationAsync(int pageNumber, int pageSize)
    {
        return Results.Ok(await orderService.GetOrdersByPaginationAsync(pageNumber, pageSize));
    }
}
