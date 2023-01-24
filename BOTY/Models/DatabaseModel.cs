using System;
using BOTY.Models.Entities;
using BOTY.Models.Tables;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BOTY.Models
{
    public class DatabaseModel
    {
        MyContext context = new();

        public MyContext ReturnContext()
        {
            return context;
        }

        public void CreateOrder(FullOrder order)
        {
            var address = new Address()
                {address = order.address, city = order.city, country = order.country, postalCode = order.postalCode};
            context.addresses.Add(address);
            context.SaveChangesAsync();
            
            var customer = new Customer()
            {
                emailName = order.emailName,
                firstName = order.firstName,
                lastName = order.lastName,
                phoneNumber = order.phoneNumber
            };
            context.customers.Add(customer);
            context.SaveChangesAsync();

            int addressId = context.addresses.ToList().Last().id;
            int customerId = context.customers.ToList().Last().Id;

            var newOrder = new Order()
            {
                supplierId = order.supplierId, customerId = customerId, dateOrdered = DateTime.Now, sale = 0,
                shippingAddressId = addressId
            };
            context.orders.Add(newOrder);
            context.SaveChangesAsync();

            int orderId = context.orders.ToList().Last().Id;
            
            List<OrderDetails> orderDetails = new();
            for (int i = 0; i < SessionCart.cart.Count; i++)
            {
                orderDetails.Add(new OrderDetails() {orderId = orderId, quantity = SessionCart.count[i], variantProductId = SessionCart.cart[i].Id});
                if (SessionCart.cart[i].sale == 0)
                    orderDetails.Last().Price = SessionCart.cart[i].price;
                else
                    orderDetails.Last().Price = SessionCart.cart[i].sale;
            }
            context.orderDetails.AddRange(orderDetails);
            context.SaveChangesAsync();
            
            SessionCart.cart.Clear();
            SessionCart.count.Clear();
        }
        
        public List<Supplier> ReturnAllSuppliers()
        {
            return context.suppliers.ToList();
        }

        public Supplier ReturnSupplierByID(int id)
        {
            return context.suppliers.ToList().Find(x => x.id == id);
        }

        public async void AddProduct(ProductImagesCategories product)
        {
            Product result = new() { name = product.name, code = product.code, description = product.description, information = product.information, manufacturer = product.manufacturer, material = product.material };
            context.products.Add(result);
            await context.SaveChangesAsync();
            int id = context.products.ToList().Find(x => x.name == product.name).Id;
            foreach (var item in product.images)
            {
                context.images.Add(new Image() { productId = id, image = item.FileName });

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileNameWithPath = Path.Combine(path, item.FileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    item.CopyTo(stream);
                }
            }
            foreach (var item in product.categories)
            {
                context.productCategories.Add(new ProductCategory() { productId = id, categoryId = item });
            }
            await context.SaveChangesAsync();
        }

        public async void UpdateProduct(Product product)
        {
            context.products.Update(product);
            await context.SaveChangesAsync();
        }

        public async void DeleteProduct(Product product)
        {
            var images = ReturnImagesByProductId(product.Id);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
            foreach (var item in images)
            {
                string fileNameWithPath = Path.Combine(path, item.image);
                File.Delete(fileNameWithPath);
            }

            context.images.RemoveRange(images);
            context.productCategories.RemoveRange(ReturnCategoryByProductId(product.Id));
            context.variants.RemoveRange(ReturnVariantsByProductId(product.Id));
            await context.SaveChangesAsync();
            context.products.Remove(product);
            await context.SaveChangesAsync();
        }

        public async void AddMaterial(Material material)
        {
            context.materials.Add(material);
            await context.SaveChangesAsync();
        }

        public async void UpdateMaterial(Material material)
        {
            context.materials.Update(material);
            await context.SaveChangesAsync();
        }

        public async void DeleteMaterial(Material material)
        {
            context.materials.Remove(material);
            await context.SaveChangesAsync();
        }

        public async void AddVariant(Variant variant)
        {
            context.variants.Add(variant);
            await context.SaveChangesAsync();
        }

        public async void UpdateVariant(Variant variant)
        {
            context.variants.Update(variant);
            await context.SaveChangesAsync();
        }

        public async void DeleteVariant(Variant variant)
        {
            context.variants.Remove(variant);
            await context.SaveChangesAsync();
        }

        public async void AddManufacturer(Manufacturer manufacturer)
        {
            context.manufacturers.Add(manufacturer);
            await context.SaveChangesAsync();
        }

        public async void UpdateManufacturer(Manufacturer manufacturer)
        {
            context.manufacturers.Update(manufacturer);
            await context.SaveChangesAsync();
        }

        public async void DeleteManufacturer(Manufacturer manufacturer)
        {
            context.manufacturers.Remove(manufacturer);
            await context.SaveChangesAsync();
        }

        public List<FullVariant> ReturnFullVariants(int id)
        {
            var variants = ReturnVariantsByProductId(id);

            List<FullVariant> result = new();
            foreach (var item in variants)
            {
                string color = FindColor(item.colorId).name;
                string size = FindSize(item.sizeId).description;
                result.Add(new FullVariant() { id = item.Id, color = color, size = size, price = item.price, sale = item.sale, vat = item.vat, stock = item.stock });
            }

            return result;
        }
        public Color FindColor(int id)
        {
            return ReturnContext().colors.ToList().Find(x => x.id == id);
        }

        public Size FindSize(int id)
        {
            return ReturnContext().sizes.ToList().Find(x => x.Id == id);
        }

        public FullProduct ReturnFullProductById(int id)
        {
            var product = ReturnContext().products.ToList().Find(x => x.Id == id);
            var categories = ReturnCategoryByProductId(id);

            FullProduct fullProduct = new() { Id = product.Id, name = product.name, description = product.description, code = product.code, information = product.information, manufacturer = ReturnManufacturerById(product.manufacturer).logo, material = ReturnMaterialById(product.material).Name };
            fullProduct.categories = new();
            foreach (var item in categories)
            {
                fullProduct.categories.Add(ReturnCategoryById(item.categoryId).name);
            }
            return fullProduct;
        }

        public List<Variant> ReturnVariantsByProductId(int id)
        {
            return ReturnContext().variants.ToList().FindAll(x => x.productId == id);
        }

        public List<Image> ReturnImagesByProductId(int id)
        {
            return ReturnContext().images.ToList().FindAll(x => x.productId == id);
        }
        public List<ProductCategory> ReturnCategoryByProductId(int id)
        {
            return ReturnContext().productCategories.ToList().FindAll(x => x.productId == id);
        }

        public Variant ReturnVariantByVariantId(int id)
        {
            return ReturnContext().variants.ToList().Find(x => x.Id == id);
        }

        public Product ReturnProductById(int id)
        {
            return ReturnContext().products.ToList().Find(x => x.Id == id);
        }

        public Material ReturnMaterialById(int id)
        {
            return ReturnContext().materials.ToList().Find(x => x.Id == id);
        }

        public Manufacturer ReturnManufacturerById(int id)
        {
            return ReturnContext().manufacturers.ToList().Find(x => x.Id == id);
        }

        public List<Color> ReturnColors(List<Variant> variants)
        {
            var colors = ReturnContext().colors.ToList();
            var result = new List<Color>();

            foreach (var item in variants)
            {
                result.Add(colors.Find(x => x.id == item.colorId));
            }
            return result;
        }

        public Color ReturnColorById(int id)
        {
            return ReturnContext().colors.ToList().Find(x => x.id == id);
        }

        public void AddColor(Color color)
        {
            context.colors.Add(color);
            context.SaveChangesAsync();
        }

        public void DeleteColor(Color color)
        {
            context.colors.Remove(color);
            context.SaveChangesAsync();
        }
        public Size ReturnSizeById(int id)
        {
            return ReturnContext().sizes.ToList().Find(x => x.Id == id);
        }

        public void AddSize(Size size)
        {
            context.sizes.Add(size);
            context.SaveChangesAsync();
        }

        public void DeleteSize(Size size)
        {
            context.sizes.Remove(size);
            context.SaveChangesAsync();
        }
        public void AddCategory(Category category)
        {
            context.categories.Add(category);
            context.SaveChangesAsync();
        }

        public void DeleteCategory(Category category)
        {
            context.categories.Remove(category);
            context.SaveChangesAsync();
        }

        public Category ReturnCategoryById(int id)
        {
            return ReturnContext().categories.ToList().Find(x => x.id == id);
        }

        public List<Size> ReturnSizes(List<Variant> variants)
        {
            var sizes = ReturnContext().sizes.ToList();
            var result = new List<Size>();

            foreach (var item in variants)
            {
                result.Add(sizes.Find(x => x.Id == item.sizeId));
            }
            return result;
        }
        
        public List<FullProduct> ReturnFullProducts()
        {
            var products = ReturnContext().products.ToList();

            List<FullProduct> result = new();
            foreach (var item in products)
            {
                string manufacturer = FindManufacturer(item.manufacturer).logo;
                string material = FindMaterial(item.material).Name;
                result.Add(new FullProduct() {Id = item.Id, name = item.name, description = item.description, information = item.information, code = item.code, manufacturer = manufacturer, material = material});
            }

            return result;
        }

        public Manufacturer FindManufacturer(int id)
        {
            return ReturnContext().manufacturers.ToList().Find(x => x.Id == id);
        }

        public Material FindMaterial(int id)
        {
            return ReturnContext().materials.ToList().Find(x => x.Id == id);
        }
        
        public void AddSupplier(Supplier supplier)
        {
            context.suppliers.Add(supplier);
            context.SaveChangesAsync();
        }

        public void DeleteSupplier(Supplier supplier)
        {
            context.suppliers.Remove(supplier);
            context.SaveChangesAsync();
        }
    }
}
