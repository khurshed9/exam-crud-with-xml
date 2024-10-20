using System.Xml.Linq;
using examWithXML.Entities;
using examWithXML.Services.CategoryService;
using examWithXML.Services.OrderService;
using examWithXML.Services.SupplierService;
using Practice;

namespace examWithXML.Services.ProductService;

public class ProductService : IProductService
{
        private readonly ICheckXmlDetailsService _check;
        private readonly string _pathData;

        public ProductService(ICheckXmlDetailsService check, IConfiguration configuration)
        {
            _pathData = configuration.GetSection(XmlElementsProduct.PathData).Value!;
            _check = check;
            
            _check.Check(XmlElementsProduct.Products);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            try
            {
                XDocument doc = XDocument.Load(_pathData);
                return doc.Elements(XmlElementsProduct.DataSource)!.Elements(XmlElementsProduct.Products)!
                    .Elements(XmlElementsProduct.Product)!
                    .Select(x => new Product
                    {
                        Id = (int)x.Element(XmlElementsProduct.ProductId)!,
                        Name = (string)x.Element(XmlElementsProduct.ProductName)!,
                        Description = (string)x.Element(XmlElementsProduct.ProductDescription)!,
                        Quantity = (int)x.Element(XmlElementsProduct.ProductQuantity)!,
                        Price = (decimal)x.Element(XmlElementsProduct.ProductPrice)!,
                        CategoryId = (int)x.Element(XmlElementsProduct.ProductCategoryId)!
                    }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            try
            {
                XDocument doc = XDocument.Load(_pathData);
                var productElem = doc.Elements(XmlElementsProduct.DataSource)!.Elements(XmlElementsProduct.Products)!
                    .Elements(XmlElementsProduct.Product)!
                    .FirstOrDefault(x => (int)x.Element(XmlElementsProduct.ProductId)! == id);

                if (productElem != null)
                {
                    return new Product
                    {
                        Id = (int)productElem.Element(XmlElementsProduct.ProductId)!,
                        Name = (string)productElem.Element(XmlElementsProduct.ProductName)!,
                        Description = (string)productElem.Element(XmlElementsProduct.ProductDescription)!,
                        Quantity = (int)productElem.Element(XmlElementsProduct.ProductQuantity)!,
                        Price = (decimal)productElem.Element(XmlElementsProduct.ProductPrice)!,
                        CategoryId = (int)productElem.Element(XmlElementsProduct.ProductCategoryId)!
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

        public async Task<bool> CreateProductAsync(Product product)
        {
            try
            {
                XDocument doc = XDocument.Load(_pathData);
            
                XElement newProduct = new XElement(XmlElementsProduct.Product,
                    new XElement(XmlElementsProduct.ProductId, product.Id),
                    new XElement(XmlElementsProduct.ProductName, product.Name),
                    new XElement(XmlElementsProduct.ProductDescription, product.Description),
                    new XElement(XmlElementsProduct.ProductQuantity, product.Quantity),
                    new XElement(XmlElementsProduct.ProductPrice, product.Price),
                    new XElement(XmlElementsProduct.ProductCategoryId, product.CategoryId));
            
                doc.Element(XmlElementsProduct.DataSource)!.Element(XmlElementsProduct.Products)!.Add(newProduct);
                doc.Save(_pathData);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            try
            {
                XDocument doc = XDocument.Load(_pathData);
                var productElem = doc.Elements(XmlElementsProduct.DataSource)!.Elements(XmlElementsProduct.Products)!
                    .Elements(XmlElementsProduct.Product)!
                    .FirstOrDefault(x => (int)x.Element(XmlElementsProduct.ProductId)! == product.Id);

                if (productElem != null)
                {
                    productElem.SetElementValue(XmlElementsProduct.ProductId, product.Id);
                    productElem.SetElementValue(XmlElementsProduct.ProductName, product.Name);
                    productElem.SetElementValue(XmlElementsProduct.ProductDescription, product.Description);
                    productElem.SetElementValue(XmlElementsProduct.ProductQuantity, product.Quantity);
                    productElem.SetElementValue(XmlElementsProduct.ProductPrice, product.Price);
                    productElem.SetElementValue(XmlElementsProduct.ProductCategoryId, product.CategoryId);
                    
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

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                XDocument doc = XDocument.Load(_pathData);
                var productElem = doc.Elements(XmlElementsProduct.DataSource)!.Elements(XmlElementsProduct.Products)!
                    .Elements(XmlElementsProduct.Product)!
                    .FirstOrDefault(x => (int)x.Element(XmlElementsProduct.ProductId)! == id);

                if (productElem != null)
                {
                    productElem.Remove();
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

        public async Task<IEnumerable<Product>> GetProductByQuantityAsync(int quantity)
        {
            try
            {
                XDocument doc = XDocument.Load(_pathData);
                return doc.Elements(XmlElementsProduct.DataSource)!.Elements(XmlElementsProduct.Products)!.Elements(
                        XmlElementsProduct.Product)!
                    .Where(x => (int)x.Element(XmlElementsProduct.ProductQuantity)! < quantity)
                    .Select(x => new Product
                    {
                        Id = (int)x.Element(XmlElementsProduct.ProductId)!,
                        Name = (string)x.Element(XmlElementsProduct.ProductName)!,
                        Quantity = (int)x.Element(XmlElementsProduct.ProductQuantity)!
                    }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<IEnumerable<GetProductOrdered5TimeM>> GetProductOrdered5TimeMAsync()
        {
            try
            {
                XDocument doc= XDocument.Load(_pathData);
                var orderedProducts =
                    from p in doc.Elements(XmlElementsProduct.DataSource)!.Elements(XmlElementsProduct.Products)!
                        .Elements(XmlElementsProduct.Product)
                    join o in doc.Elements(XmlElementsOrder.DataSource)!.Elements(XmlElementsOrder.Orders)!
                            .Elements(XmlElementsOrder.Order) on (int)p.Element(XmlElementsProduct.ProductId) equals
                        (int)o.Element(XmlElementsOrder.ProductId)
                    group o by p
                    into g
                    where g.Count() > 5
                    select new GetProductOrdered5TimeM()
                    {
                        ProductId = (int)g.Key.Element(XmlElementsProduct.ProductId),
                        ProductName = (string)g.Key.Element(XmlElementsProduct.ProductName),
                        TotalOrdered = g.Count()
                    };
                
                return orderedProducts.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<GetProductCategorySupplier> GetProductCategorySupplierAsync(int productId)
        {
            try
            {
                XDocument doc = XDocument.Load(_pathData);
                var productDetails =
                    from p in doc.Elements(XmlElementsProduct.DataSource)!
                        .Elements(XmlElementsProduct.Products)!
                        .Elements(XmlElementsProduct.Product)
                    where (int)p.Element(XmlElementsProduct.ProductId) == productId
                    join c in doc.Elements(XmlElementsCategory.DataSource)!
                        .Elements(XmlElementsCategory.Categories)!
                        .Elements(XmlElementsCategory.Category) on (int)p.Element(XmlElementsProduct.ProductCategoryId) equals (int)c.Element(XmlElementsCategory.CategoryId)
                    join o in doc.Elements(XmlElementsOrder.DataSource)!
                        .Elements(XmlElementsOrder.Orders)!
                        .Elements(XmlElementsOrder.Order) on (int)p.Element(XmlElementsProduct.ProductId) equals (int)o.Element(XmlElementsOrder.ProductId)
                    join s in doc.Elements(XmlElementsSupplier.DataSource)!
                        .Elements(XmlElementsSupplier.Suppliers)!
                        .Elements(XmlElementsSupplier.Supplier) on (int)o.Element(XmlElementsOrder.SupplierId) equals (int)s.Element(XmlElementsSupplier.SupplierId)
                    group new { p, c, s } by p
                    into g
                    select new GetProductCategorySupplier
                    {
                        ProductId = (int)g.Key.Element(XmlElementsProduct.ProductId),
                        ProductName = (string)g.Key.Element(XmlElementsProduct.ProductName),
                        CategoryName = (string)g.Select(x => x.c.Element(XmlElementsCategory.CategoryName)).FirstOrDefault(),
                        SupplierName = (string)g.Select(x => x.s.Element(XmlElementsSupplier.SupplierName)).FirstOrDefault(),
                        TotalOrders = g.Count()
                    };

                return productDetails.FirstOrDefault();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetProductByCategoryAndPriceAsync(int categoryId, string sortOrder)
        {
            try
            {
                XDocument doc = XDocument.Load(_pathData);
                var product = doc.Elements(XmlElementsProduct.DataSource)!.Elements(XmlElementsProduct.Products)!
                    .Elements(XmlElementsProduct.Product)!
                    .Where(x => (int)x.Element(XmlElementsProduct.ProductCategoryId) == categoryId);

               product = sortOrder == "asc"
                   ? product.OrderBy(x=>(decimal)x.Element(XmlElementsProduct.ProductPrice)!)
                   : product.OrderByDescending(x => (decimal)x.Element(XmlElementsProduct.ProductPrice)!);
               
               return product.Select(x => new Product
               {
                   Id = (int)x.Element(XmlElementsProduct.ProductId)!,
                   Name = (string)x.Element(XmlElementsProduct.ProductName)!,
                   Description = (string)x.Element(XmlElementsProduct.ProductDescription)!,
                   Quantity = (int)x.Element(XmlElementsProduct.ProductQuantity)!,
                   Price = (decimal)x.Element(XmlElementsProduct.ProductPrice)!,
                   CategoryId = (int)x.Element(XmlElementsProduct.ProductCategoryId)!
               }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        
        public async Task<IEnumerable<GetProductsByPaginationAsync>> GetProductsWithDetailsAsync(int pageNumber, int pageSize)
        {
            try
            {
                XDocument doc = XDocument.Load(_pathData);
        
                var products = from c in doc.Elements(XmlElementsCategory.DataSource)!
                        .Elements(XmlElementsCategory.Categories)!
                        .Elements(XmlElementsCategory.Category)!
                    join p in doc.Elements(XmlElementsProduct.DataSource)!
                            .Elements(XmlElementsProduct.Products)!
                            .Elements(XmlElementsProduct.Product) on 
                        (int)c.Element(XmlElementsCategory.CategoryId) equals 
                        (int)p.Element(XmlElementsProduct.ProductCategoryId)
                    join o in doc.Elements(XmlElementsOrder.DataSource)!
                        .Elements(XmlElementsOrder.Orders)!
                        .Elements(XmlElementsOrder.Order)! on 
                        (int)p.Element(XmlElementsProduct.ProductId) equals (int)o.Element(XmlElementsOrder.ProductId)
                    join s in doc.Elements(XmlElementsSupplier.DataSource)!
                        .Elements(XmlElementsSupplier.Suppliers)!
                        .Elements(XmlElementsSupplier.Supplier)! on 
                        (int)o.Element(XmlElementsOrder.SupplierId) equals (int)s.Element(XmlElementsSupplier.SupplierId) 
                    select new GetProductsByPaginationAsync()
                    {
                        Id = (int)p.Element(XmlElementsProduct.ProductId),
                        ProductName = (string)p.Element(XmlElementsProduct.ProductName),
                        CategoryName = (string)c.Element(XmlElementsCategory.CategoryName),
                        SupplierName = (string)s.Element(XmlElementsSupplier.SupplierName),
                        Price = (decimal)p.Element(XmlElementsProduct.ProductPrice)
                    };

                var paginatedProducts = products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                return paginatedProducts;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
}

public static class XmlElementsProduct
{
    public const string PathData = "PathData";
    public const string DataSource = "source";
    public const string Products = "products";
    public const string Product = "product";    
    public const string ProductId = "id";
    public const string ProductName = "name";
    public const string ProductDescription = "description";
    public const string ProductQuantity = "quantity";
    public const string ProductPrice = "price";
    public const string ProductCategoryId = "categoryId";
}