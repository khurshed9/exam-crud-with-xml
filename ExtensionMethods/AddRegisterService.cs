
using examWithXML.Services.CategoryService;
using examWithXML.Services.OrderService;
using examWithXML.Services.ProductService;
using examWithXML.Services.SupplierService;
using Practice;

namespace examEfCore.ExtensionMethods;

public static class AddRegisterService
{
    public static void AddService(this IServiceCollection serviceCollection)
    {
        string filePath = "appsettings.json";
    
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(filePath)
            .Build();
        
        serviceCollection.AddTransient<IProductService, ProductService>();
        serviceCollection.AddSingleton<ICheckXmlDetailsService, CheckXmlDetailsService>();
        serviceCollection.AddSingleton<IConfiguration>(configuration);
        serviceCollection.AddTransient<ICategoryService, CategoryService>();
        serviceCollection.AddTransient<ISupplierService, SupplierService>();
        serviceCollection.AddTransient<IOrderService, OrderService>();

    }
}