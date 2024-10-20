using System.Xml.Linq;
using examWithXML.Entities;
using examWithXML.Entities.Queries;
using examWithXML.Services.ProductService;
using Practice;

namespace examWithXML.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly ICheckXmlDetailsService _check;
    private readonly string _pathData;

    public CategoryService(ICheckXmlDetailsService check, IConfiguration configuration)
    {
        _pathData = configuration.GetSection(XmlElementsCategory.PathData).Value!;
        _check = check;
        
        _check.Check(XmlElementsCategory.Categories);
    }
    
    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        try
        {
            XDocument doc = XDocument.Load(_pathData);

            return doc.Elements(XmlElementsCategory.DataSource)!.Elements(XmlElementsCategory.Categories)!
                .Elements(XmlElementsCategory.Category)!
                .Select(x => new Category
                {
                    Id = (int)x.Element(XmlElementsCategory.CategoryId)!,
                    Name = (string)x.Element(XmlElementsCategory.CategoryName)!,
                    Description = (string)x.Element(XmlElementsCategory.CategoryDescription)!
                }).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        try
        {
            XDocument doc = XDocument.Load(_pathData);
            var categoryElement = doc.Elements(XmlElementsCategory.DataSource)!.Elements(XmlElementsCategory.Categories)!
                .Elements(XmlElementsCategory.Category)!
                .FirstOrDefault(x => (int)x.Element(XmlElementsCategory.CategoryId)! == id);

            if (categoryElement != null)
            {
                return new Category
                {
                    Id = (int)categoryElement.Element(XmlElementsCategory.CategoryId)!,
                    Name = (string)categoryElement.Element(XmlElementsCategory.CategoryName)!,
                    Description = (string)categoryElement.Element(XmlElementsCategory.CategoryDescription)!
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

    public async Task<bool> CreateCategoryAsync(Category category)
    {
        try
        {
            XDocument doc = XDocument.Load(_pathData);

            XElement element = new XElement(XmlElementsCategory.Category,
                new XElement(XmlElementsCategory.CategoryId,category.Id),
                new XElement(XmlElementsCategory.CategoryName,category.Name),
                new XElement(XmlElementsCategory.CategoryDescription,category.Description)
            );
            
            doc.Element(XmlElementsCategory.DataSource)!.Element(XmlElementsCategory.Categories)!.Add(element);
            doc.Save(_pathData);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        try
        {
            XDocument doc = XDocument.Load(_pathData);
            var categoryElem = doc.Elements(XmlElementsCategory.DataSource)!.Elements(XmlElementsCategory.Categories)!
                .Elements(XmlElementsCategory.Category)!
                .FirstOrDefault(x => (int)x.Element(XmlElementsCategory.CategoryId)! == category.Id);

            if (categoryElem != null)
            {
                categoryElem.SetElementValue(XmlElementsCategory.CategoryId,category.Id);
                categoryElem.SetElementValue(XmlElementsCategory.CategoryName,category.Name);
                categoryElem.SetElementValue(XmlElementsCategory.CategoryDescription,category.Description);
                
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

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        try
        {
            XDocument doc = XDocument.Load(_pathData);
            var categoryElem = doc.Elements(XmlElementsCategory.DataSource)!.Elements(XmlElementsCategory.Categories)!
                .Elements(XmlElementsCategory.Category)!
                .FirstOrDefault(x => (int)x.Element(XmlElementsCategory.CategoryId)! == id);

            if (categoryElem != null)
            {
                categoryElem.Remove();
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

    public async Task<IEnumerable<GetCategoryWithProd>> GetCategoryWithProdAsync()
    {
        try
        {
            XDocument doc = XDocument.Load(_pathData);
            var getElem =
                from c in doc.Elements(XmlElementsCategory.DataSource)!
                    .Elements(XmlElementsCategory.Categories)!
                    .Elements(XmlElementsCategory.Category)
                join p in doc.Elements(XmlElementsProduct.DataSource)!
                        .Elements(XmlElementsProduct.Products)!
                        .Elements(XmlElementsProduct.Product) 
                    on (int)c.Element(XmlElementsCategory.CategoryId) equals (int)p.Element(XmlElementsProduct.ProductCategoryId)
                group p by c into g
                select new GetCategoryWithProd
                {
                    Id = (int)g.Key.Element(XmlElementsCategory.CategoryId),
                    CategoryName = (string)g.Key.Element(XmlElementsCategory.CategoryName),
                    TotalAmountProduct = g.Count()
                };

            return getElem;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}

public static class XmlElementsCategory
{
    public const string PathData = "PathData";
    public const string DataSource = "source";
    public const string Categories = "categories";
    public const string Category = "category";
    public const string CategoryId = "id";
    public const string CategoryName = "name";
    public const string CategoryDescription = "description";
}