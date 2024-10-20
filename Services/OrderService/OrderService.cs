using System.Xml.Linq;
using examWithXML.Entities;
using examWithXML.Entities.Queries;
using examWithXML.Services.CategoryService;
using examWithXML.Services.SupplierService;
using Practice;

namespace examWithXML.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly ICheckXmlDetailsService _check;
    private readonly string _pathData;

    public OrderService(ICheckXmlDetailsService check, IConfiguration configuration)
    {
        _pathData = configuration.GetSection(XmlElementsOrder.PathData).Value!;
        _check = check;
        
        _check.Check(XmlElementsOrder.Orders);
    }
    
    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        try
        {
            XDocument doc = XDocument.Load(_pathData);
            return doc.Elements(XmlElementsOrder.DataSource)!.Elements(XmlElementsOrder.Orders)!
                .Elements(XmlElementsOrder.Order)!
                .Select(x => new Order
                {
                    Id = (int)x.Element(XmlElementsOrder.OrderId)!,
                    Quantity = (int)x.Element(XmlElementsOrder.OrderQuantity)!,
                    OrderDate = (DateTime)x.Element(XmlElementsOrder.OrderDate)!,
                    Status = (string)x.Element(XmlElementsOrder.OrderStatus)!,
                    ProductId = (int)x.Element(XmlElementsOrder.ProductId)!,
                    SupplierId = (int)x.Element(XmlElementsOrder.SupplierId)!
                }).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        try
        {
            XDocument doc = XDocument.Load(_pathData);
            var orderElem = doc.Elements(XmlElementsOrder.DataSource)!.Elements(XmlElementsOrder.Orders)!
                .Elements(XmlElementsOrder.Order)!
                .FirstOrDefault(x => (int)x.Element(XmlElementsOrder.OrderId)! == id);

            if (orderElem != null)
            {
                return new Order
                {
                    Id = (int)orderElem.Element(XmlElementsOrder.OrderId)!,
                    Quantity = (int)orderElem.Element(XmlElementsOrder.OrderQuantity)!,
                    OrderDate = (DateTime)orderElem.Element(XmlElementsOrder.OrderDate)!,
                    Status = (string)orderElem.Element(XmlElementsOrder.OrderStatus)!,
                    ProductId = (int)orderElem.Element(XmlElementsOrder.ProductId)!,
                    SupplierId = (int)orderElem.Element(XmlElementsOrder.SupplierId)!
                };
            }
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<bool> CreateOrderAsync(Order order)
    {
        try
        {
            XDocument doc = XDocument.Load(_pathData);
            XElement newOrder = new XElement(XmlElementsOrder.Order,
                new XElement(XmlElementsOrder.OrderId, order.Id),
                new XElement(XmlElementsOrder.OrderQuantity, order.Quantity),
                new XElement(XmlElementsOrder.OrderDate, order.OrderDate),
                new XElement(XmlElementsOrder.OrderStatus, order.Status),
                new XElement(XmlElementsOrder.ProductId, order.ProductId),
                new XElement(XmlElementsOrder.SupplierId, order.SupplierId)
    
            );

            doc.Element(XmlElementsOrder.DataSource)!.Element(XmlElementsOrder.Orders)!.Add(newOrder);
            doc.Save(_pathData);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<bool> UpdateOrderAsync(Order order)
    {
        try
        {
            XDocument doc = XDocument.Load(_pathData);
            var orderElem = doc.Elements(XmlElementsOrder.DataSource)!.Elements(XmlElementsOrder.Orders)!
                .Elements(XmlElementsOrder.Order)!
                .FirstOrDefault(x => (int)x.Element(XmlElementsOrder.OrderId)! == order.Id);

            if (orderElem != null)
            {
                orderElem.SetElementValue(XmlElementsOrder.OrderQuantity, order.Quantity);
                orderElem.SetElementValue(XmlElementsOrder.OrderDate, order.OrderDate);
                orderElem.SetElementValue(XmlElementsOrder.OrderStatus, order.Status);
                orderElem.SetElementValue(XmlElementsOrder.ProductId, order.ProductId);
                orderElem.SetElementValue(XmlElementsOrder.SupplierId, order.SupplierId);
                
                doc.Save(_pathData);
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        try
        {
            XDocument doc = XDocument.Load(_pathData);
            var orderElem = doc.Elements(XmlElementsOrder.DataSource)!.Elements(XmlElementsOrder.Orders)!
                .Elements(XmlElementsOrder.Order)!
                .FirstOrDefault(x => (int)x.Element(XmlElementsOrder.OrderId)! == id);

            if (orderElem != null)
            {
                orderElem.Remove();
                doc.Save(_pathData);
                return true;
            }
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Order>> GetOrderByOrderDateAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            XDocument doc= XDocument.Load(_pathData);
            return doc.Elements(XmlElementsOrder.DataSource)!.Elements(XmlElementsOrder.Orders)!.Elements(XmlElementsOrder
                    .Order)!
                .Where(x => (DateTime)x.Element(XmlElementsOrder.OrderDate)! >= startDate &&
                            (DateTime)x.Element(XmlElementsOrder.OrderDate)! <= endDate)
                .Select(x => new Order
                {
                    Id = (int)x.Element(XmlElementsOrder.OrderId)!,
                    Quantity = (int)x.Element(XmlElementsOrder.OrderQuantity)!,
                    OrderDate = (DateTime)x.Element(XmlElementsOrder.OrderDate)!,
                    Status = (string)x.Element(XmlElementsOrder.OrderStatus)!,
                    ProductId = (int)x.Element(XmlElementsOrder.ProductId)!,
                    SupplierId = (int)x.Element(XmlElementsOrder.SupplierId)!
                }).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Order>> GetOrdersBySupplierAndStatusAsync(int supplierId, string status)
    {
        try
        {
            XDocument doc = XDocument.Load(_pathData);
            var orderElements = doc.Elements(XmlElementsOrder.DataSource)!
                .Elements(XmlElementsOrder.Orders)!
                .Elements(XmlElementsOrder.Order)!
                .Where(x => (int)x.Element(XmlElementsOrder.SupplierId)! == supplierId &&
                            (string)x.Element(XmlElementsOrder.OrderStatus)! == status);

            var orders = orderElements.Select(orderElement => new Order
            {
                Id = (int)orderElement.Element(XmlElementsOrder.OrderId)!,
                Quantity = (int)orderElement.Element(XmlElementsOrder.OrderQuantity)!,
                OrderDate = (DateTime)orderElement.Element(XmlElementsOrder.OrderDate)!,
                Status = (string)orderElement.Element(XmlElementsOrder.OrderStatus)!,
                ProductId = (int)orderElement.Element(XmlElementsOrder.ProductId)!,
                SupplierId = (int)orderElement.Element(XmlElementsOrder.SupplierId)!
            }).ToList(); 

            return orders;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    
    public async Task<IEnumerable<Order>> GetOrdersByPaginationAsync(int pageNumber, int pageSize)
    {
        try
        {
            XDocument doc = XDocument.Load(_pathData);
            var orders = doc.Elements(XmlElementsOrder.DataSource)!
                .Elements(XmlElementsOrder.Orders)!
                .Elements(XmlElementsOrder.Order)!
                .Select(order => new Order
                {
                    Id = (int)order.Element(XmlElementsOrder.OrderId)!,
                    Quantity = (int)order.Element(XmlElementsOrder.OrderQuantity)!,
                    OrderDate = (DateTime)order.Element(XmlElementsOrder.OrderDate)!,
                    Status = (string)order.Element(XmlElementsOrder.OrderStatus)!,
                    ProductId = (int)order.Element(XmlElementsOrder.ProductId)!,
                    SupplierId = (int)order.Element(XmlElementsOrder.SupplierId)!
                });

            var paginatedOrders = orders.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return paginatedOrders;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    
}


public static class XmlElementsOrder
{
    public const string PathData = "PathData";
    public const string DataSource = "source";
    public const string Orders = "orders";
    public const string Order = "order";
    public const string OrderId = "id";
    public const string OrderQuantity = "quantity";
    public const string OrderDate = "orderdate";
    public const string OrderStatus = "status";
    public const string ProductId = "produciId";
    public const string SupplierId = "supplierid";
}