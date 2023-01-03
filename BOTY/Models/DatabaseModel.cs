using BOTY.Models.Entities;
using BOTY.Models.Tables;
using System.Collections.Generic;
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

        public async void AddProduct(Product product)
        {
            context.products.Add(product);
            await context.SaveChangesAsync();
        }

        public async void UpdateProduct(Product product)
        {
            context.products.Update(product);
            await context.SaveChangesAsync();
        }

        public async void DeleteProduct(Product product)
        {
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

        public List<Variant> ReturnVariantsByProductId(int id)
        {
            return ReturnContext().variants.ToList().FindAll(x => x.productId == id);
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
    }
}
