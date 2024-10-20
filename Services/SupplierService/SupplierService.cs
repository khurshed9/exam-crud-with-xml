using System.Xml.Linq;
using examWithXML.Entities;
using examWithXML.Services.OrderService;
using examWithXML.Services.ProductService;
using Practice;

namespace examWithXML.Services.SupplierService;

    public class SupplierService : ISupplierService
    {
        private readonly ICheckXmlDetailsService _check;
        private readonly string _pathData;

        public SupplierService(ICheckXmlDetailsService check, IConfiguration configuration)
        {
            _pathData = configuration.GetSection(XmlElementsSupplier.PathData).Value!;
            _check = check;
                
            _check.Check(XmlElementsSupplier.Suppliers);
        }
        
        public async Task<IEnumerable<Supplier>> GetSuppliersAsync()
        {
            try
            {
                XDocument doc = XDocument.Load(_pathData);
                return doc.Elements(XmlElementsSupplier.DataSource)!.Elements(XmlElementsSupplier.Suppliers)!
                    .Elements(XmlElementsSupplier.Supplier)!
                    .Select(x => new Supplier
                    {
                        Id = (int)x.Element(XmlElementsSupplier.SupplierId)!,
                        Name = (string)x.Element(XmlElementsSupplier.SupplierName)!,
                        ContactPerson = (string)x.Element(XmlElementsSupplier.SupplierContactPerson)!,
                        Email = (string)x.Element(XmlElementsSupplier.SupplierEmail)!,
                        Phone = (string)x.Element(XmlElementsSupplier.SupplierPhone)!   
                    }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<Supplier?> GetSupplierByIdAsync(int id)
        {
            try
            {
                XDocument doc = XDocument.Load(_pathData);
                var supplierElem = doc.Elements(XmlElementsSupplier.DataSource)!.Elements(XmlElementsSupplier.Suppliers)!
                    .Elements(XmlElementsSupplier.Supplier)!
                    .FirstOrDefault(x => (int)x.Element(XmlElementsSupplier.SupplierId)! == id);

                if (supplierElem != null)
                {
                    return new Supplier
                    {
                        Id = (int)supplierElem.Element(XmlElementsSupplier.SupplierId)!,
                        Name = (string)supplierElem.Element(XmlElementsSupplier.SupplierName)!,
                        ContactPerson = (string)supplierElem.Element(XmlElementsSupplier.SupplierContactPerson)!,
                        Email = (string)supplierElem.Element(XmlElementsSupplier.SupplierEmail)!,
                        Phone = (string)supplierElem.Element(XmlElementsSupplier.SupplierPhone)!
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

        public async Task<bool> CreateSupplierAsync(Supplier supplier)
        {
            try
            {
                XDocument doc = XDocument.Load(_pathData);
                XElement newSupplier = new XElement(XmlElementsSupplier.Supplier,
                    new XElement(XmlElementsSupplier.SupplierId, supplier.Id),
                    new XElement(XmlElementsSupplier.SupplierName, supplier.Name),
                    new XElement(XmlElementsSupplier.SupplierContactPerson, supplier.ContactPerson),
                    new XElement(XmlElementsSupplier.SupplierEmail, supplier.Email),
                    new XElement(XmlElementsSupplier.SupplierPhone, supplier.Phone));

                doc.Element(XmlElementsSupplier.DataSource)!.Element(XmlElementsSupplier.Suppliers)!.Add(newSupplier);
                doc.Save(_pathData);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<bool> UpdateSupplierAsync(Supplier supplier)
        {
            try
            {
                XDocument doc = XDocument.Load(_pathData);
                var supplierElem = doc.Elements(XmlElementsSupplier.DataSource)!.Elements(XmlElementsSupplier.Suppliers)!
                    .Elements(XmlElementsSupplier.Supplier)!
                    .FirstOrDefault(x => (int)x.Element(XmlElementsSupplier.SupplierId)! == supplier.Id);

                if (supplierElem != null)
                {
                    supplierElem.SetElementValue(XmlElementsSupplier.SupplierId, supplier.Id);
                    supplierElem.SetElementValue(XmlElementsSupplier.SupplierName, supplier.Name);
                    supplierElem.SetElementValue(XmlElementsSupplier.SupplierContactPerson, supplier.ContactPerson);
                    supplierElem.SetElementValue(XmlElementsSupplier.SupplierEmail, supplier.Email);
                    supplierElem.SetElementValue(XmlElementsSupplier.SupplierPhone, supplier.Phone);
                    
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

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            try
            {
                XDocument doc = XDocument.Load(_pathData);
                var supplierElem = doc.Elements(XmlElementsSupplier.DataSource)!.Elements(XmlElementsSupplier.Suppliers)!
                    .Elements(XmlElementsSupplier.Supplier)!
                    .FirstOrDefault(x => (int)x.Element(XmlElementsSupplier.SupplierId)! == id);

                if (supplierElem != null)
                {
                    supplierElem.Remove();
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

        public async Task<IEnumerable<GetSupplierByProductQuantity>> GetSupplierByProductQuantityAsync(int productQuantity)
        {
            try
            {
                XDocument doc = XDocument.Load(_pathData);
                var supplierElem = from s in doc.Elements(XmlElementsSupplier.DataSource)!.Elements(XmlElementsSupplier.Suppliers)! 
                    .Elements(XmlElementsSupplier.Supplier)!
                    join o in doc.Elements(XmlElementsOrder.DataSource)!.Elements(XmlElementsOrder.Orders)!.Elements(XmlElementsOrder.Order)! on (int)s.Element(XmlElementsSupplier.SupplierId) equals (int)o.Element(XmlElementsOrder.SupplierId)
                    join p in doc.Elements(XmlElementsProduct.DataSource)!.Elements(XmlElementsProduct.Products)!.Elements(XmlElementsProduct.Product)! on (int)o.Element(XmlElementsOrder.ProductId) equals (int)p.Element(XmlElementsProduct.ProductId)
                    group p by s into g
                        select new GetSupplierByProductQuantity
                        {
                            Id = (int)g.Key.Element(XmlElementsSupplier.SupplierId)!,
                            Name = (string)g.Key.Element(XmlElementsSupplier.SupplierName)!,
                            ContactPerson = (string)g.Key.Element(XmlElementsSupplier.SupplierContactPerson)!,
                            Email = (string)g.Key.Element(XmlElementsSupplier.SupplierEmail)!,
                            Phone = (string)g.Key.Element(XmlElementsSupplier.SupplierPhone)!,
                            ProductQuantity = g.Count()
                        };

                return supplierElem.Where(x => x.ProductQuantity >= productQuantity).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }

    public static class XmlElementsSupplier
    {
        public const string PathData = "PathData";
        public const string DataSource = "source";
        public const string Suppliers = "suppliers";
        public const string Supplier = "supplier";
        public const string SupplierId = "id";
        public const string SupplierName = "name";
        public const string SupplierContactPerson = "contactperson";
        public const string SupplierEmail = "email";
        public const string SupplierPhone = "phone";
    }