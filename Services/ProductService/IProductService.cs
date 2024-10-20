using examWithXML.Entities;

namespace examWithXML.Services.ProductService;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsAsync();
    
    Task<Product?> GetProductByIdAsync(int id);
    
    Task<bool> CreateProductAsync(Product product);
    
    Task<bool> UpdateProductAsync(Product product);
    
    Task<bool> DeleteProductAsync(int id);
    
    Task<IEnumerable<Product>> GetProductByQuantityAsync(int quantity);

    Task<IEnumerable<GetProductOrdered5TimeM>> GetProductOrdered5TimeMAsync();

    Task<GetProductCategorySupplier> GetProductCategorySupplierAsync(int productId);

    Task<IEnumerable<Product>> GetProductByCategoryAndPriceAsync(int categoryId,string sortOrder);

    Task<IEnumerable<GetProductsByPaginationAsync>> GetProductsWithDetailsAsync(int pageNumber, int pageSize);
}