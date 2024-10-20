using System.Xml.Linq;
using examWithXML.Services.ProductService;

namespace Practice;

public class CheckXmlDetailsService : ICheckXmlDetailsService
{
   private readonly string _pathData;
   
   public CheckXmlDetailsService(IConfiguration configuration)
   {
      _pathData = configuration.GetSection(XmlElements.PathData).Value!;
   }
   
   public void Check(string elemName)
   {
      if (!File.Exists(_pathData) || new FileInfo(_pathData).Length == 0)
      {
         CreateInitialXml();
      }
      else
      {
         XDocument xDocument = XDocument.Load(_pathData);
         XElement? sourceElement = xDocument.Element(XmlElements.DataSource);

         if (sourceElement == null)
         {
            sourceElement = new XElement(XmlElements.DataSource);
            xDocument.Add(sourceElement);
         }

         XElement? targetElement = sourceElement.Element(elemName);
         if (targetElement == null)
         {
            targetElement = new XElement(elemName);
            sourceElement.Add(targetElement);
         }

         xDocument.Save(_pathData);
      }
   }


   private void CreateInitialXml()
   {
      XDocument xDocument = new XDocument();
      xDocument.Declaration = new XDeclaration(XmlElements.XmlVersion, XmlElements.UTF, XmlElements.Bool);
    
      XElement xElement = new XElement(XmlElements.DataSource,
         new XElement(XmlElementsProduct.Products)
      );
    
      xDocument.Add(xElement);
      xDocument.Save(_pathData);
   }
}

file static class XmlElements
{
   public const string DataSource = "source";
   public const string PathData = "PathData";
   public const string XmlVersion = "1.0";
   public const string UTF = "UTF-8";
   public const string Bool = "true";
}