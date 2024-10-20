using examWithXML.Entities;
using examWithXML.Entities.Queries;

namespace examWithXML.Services.OrderService;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int id);
    Task<bool> CreateOrderAsync(Order order);
    Task<bool> UpdateOrderAsync(Order order);
    Task<bool> DeleteOrderAsync(int id);

    Task<IEnumerable<Order>> GetOrderByOrderDateAsync(DateTime startDate, DateTime endDate);

    Task<IEnumerable<Order>> GetOrdersBySupplierAndStatusAsync(int supplierId, string status);

    Task<IEnumerable<Order>> GetOrdersByPaginationAsync(int pageNumber, int pageSize);
}