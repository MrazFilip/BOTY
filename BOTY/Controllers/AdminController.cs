using BOTY.Models;
using BOTY.Models.Entities;
using BOTY.Models.Models;
using BOTY.Models.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BOTY.Controllers
{
    public class AdminController : BaseController
    {
        private int productId { get; set; }
        private DatabaseModel databaseModel = new();

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            usersService.admins = databaseModel.ReturnContext().logins.ToList();

            if (this.usersService.Authenticate(model.Login, model.Password))
            {
                this.HttpContext.Session.SetString("login", model.Login);
                return RedirectToAction("Products", "Admin");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            this.HttpContext.Session.Remove("login");
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Index()
        {
            if (this.HttpContext.Session.GetString("login") != null)
                return View();
            else
                return RedirectToAction("Login", "Admin");
        }
        public IActionResult Products()
        {
            if (this.HttpContext.Session.GetString("login") != null)
            {
                ViewBag.Products = databaseModel.ReturnFullProducts();
                return View();
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        public IActionResult Products(Product product)
        {
            databaseModel.DeleteProduct(product);
            return View();
        }

        [HttpGet]
        public IActionResult ShoeCreate()
        {
            if (this.HttpContext.Session.GetString("login") != null)
            {
                ViewBag.Materials = databaseModel.ReturnContext().materials;
                ViewBag.Manufacturers = databaseModel.ReturnContext().manufacturers;
                ViewBag.Categories = databaseModel.ReturnContext().categories;
                ProductImagesCategories product = new ProductImagesCategories();
                return View(product);
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        public IActionResult ShoeCreate(ProductImagesCategories product)
        {
            databaseModel.AddProduct(product);
            return RedirectToAction("Products");
        }

        //TODO
        /*
        [HttpGet]
        public IActionResult ShoeEdit(int id)
        {
            if (this.HttpContext.Session.GetString("login") != null)
            {
                ViewBag.Materials = databaseModel.ReturnContext().materials;
                ViewBag.Manufacturers = databaseModel.ReturnContext().manufacturers;
                ProductImagesCategories product = databaseModel.ReturnFullProductById(id);
                return View(prprooduct);
            }
            else
                return RedirectToAction("Login", "Admin");
        }*/

        [HttpPost]
        public IActionResult ShoeEdit(Product product)
        {
            databaseModel.UpdateProduct(product);
            return RedirectToAction("Products");
        }

        public IActionResult ShoeDelete(int id)
        {
            databaseModel.DeleteProduct(databaseModel.ReturnProductById(id));
            return RedirectToAction("Products");
        }

        public IActionResult Materials()
        {
            if (this.HttpContext.Session.GetString("login") != null)
            {
                ViewBag.Materials = databaseModel.ReturnContext().materials;
                return View();
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        [HttpGet]
        public IActionResult MaterialCreate()
        {
            if (this.HttpContext.Session.GetString("login") != null)
            {
                Material product = new();
                return View(product);
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        public IActionResult MaterialCreate(Material material)
        {
            databaseModel.AddMaterial(material);
            return RedirectToAction("Materials");
        }

        [HttpGet]
        public IActionResult MaterialEdit(int id)
        {
            if (this.HttpContext.Session.GetString("login") != null)
            {
                Material material = databaseModel.ReturnMaterialById(id);
                return View(material);
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        public IActionResult MaterialEdit(Material material)
        {
            databaseModel.UpdateMaterial(material);
            return RedirectToAction("Materials");
        }

        public IActionResult MaterialDelete(int id)
        {
            databaseModel.DeleteMaterial(databaseModel.ReturnMaterialById(id));
            return RedirectToAction("Materials");
        }

        public IActionResult Manufacturers()
        {
            if (this.HttpContext.Session.GetString("login") != null)
            {
                ViewBag.Manufacturers = databaseModel.ReturnContext().manufacturers;
                return View();
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        [HttpGet]
        public IActionResult ManufacturerCreate()
        {
            if (this.HttpContext.Session.GetString("login") != null)
            {
                Manufacturer manufacturer = new();
                return View(manufacturer);
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        public IActionResult ManufacturerCreate(Manufacturer manufacter)
        {
            databaseModel.AddManufacturer(manufacter);
            return RedirectToAction("Manufacturers");
        }

        [HttpGet]
        public IActionResult ManufacturerEdit(int id)
        {
            if (this.HttpContext.Session.GetString("login") != null)
            {
                Manufacturer manufacturer = databaseModel.ReturnManufacturerById(id);
                return View(manufacturer);
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        public IActionResult ManufacturerEdit(Manufacturer manufacturer)
        {
            databaseModel.UpdateManufacturer(manufacturer);
            return RedirectToAction("Materials");
        }

        public IActionResult ManufacturerDelete(int id)
        {
            databaseModel.DeleteManufacturer(databaseModel.ReturnManufacturerById(id));
            return RedirectToAction("Manufacturers");
        }

        public IActionResult Variant(int id)
        {
            if (this.HttpContext.Session.GetString("login") != null)
            {
                ViewBag.ID = id;
                ViewBag.Variants = databaseModel.ReturnFullVariants(id);
                return View();
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        [HttpGet]
        public IActionResult VariantCreate(int productId)
        {
            if (this.HttpContext.Session.GetString("login") != null)
            {
                ViewBag.ID = productId;
                ViewBag.Colors = databaseModel.ReturnContext().colors;
                ViewBag.Sizes = databaseModel.ReturnContext().sizes;
                Variant variant = new Variant();
                return View(variant);
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        public IActionResult VariantCreate(Variant variant)
        {
            databaseModel.AddVariant(variant);
            return RedirectToAction("Variant", new { id = variant.productId });
        }

        [HttpGet]
        public IActionResult VariantEdit(int variantId, int productId)
        {
            if (this.HttpContext.Session.GetString("login") != null)
            {
                ViewBag.Colors = databaseModel.ReturnContext().colors;
                ViewBag.Sizes = databaseModel.ReturnContext().sizes;
                Variant variant = databaseModel.ReturnVariantByVariantId(variantId);
                ViewBag.VariantID = variant.Id;
                ViewBag.ProductID = variant.productId;
                this.productId = variant.productId;
                return View(variant);
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        public IActionResult VariantEdit(Variant variant)
        {
            databaseModel.UpdateVariant(variant);
            return RedirectToAction("Variant", new { id = variant.productId });
        }

        public IActionResult VariantDelete(int id, int productId)
        {
            databaseModel.DeleteVariant(databaseModel.ReturnVariantByVariantId(id));
            return RedirectToAction("Variant", new { id = productId });
        }

        public IActionResult Colors()
        {
            if (this.HttpContext.Session.GetString("login") != null)
            {
                ViewBag.Colors = databaseModel.ReturnContext().colors;
                return View();
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        public IActionResult ColorCreate(Color color)
        {
            databaseModel.AddColor(color);
            return RedirectToAction("Colors");
        }

        public IActionResult ColorDelete(int id)
        {
            databaseModel.DeleteColor(databaseModel.ReturnColorById(id));
            return RedirectToAction("Colors");
        }

        public IActionResult Sizes()
        {
            if (this.HttpContext.Session.GetString("login") != null)
            {
                ViewBag.Sizes = databaseModel.ReturnContext().sizes;
                return View();
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        public IActionResult SizeCreate(Size size)
        {
            databaseModel.AddSize(size);
            return RedirectToAction("Sizes");
        }

        public IActionResult SizeDelete(int id)
        {
            databaseModel.DeleteSize(databaseModel.ReturnSizeById(id));
            return RedirectToAction("Sizes");
        }

        public IActionResult Categories()
        {
            if (this.HttpContext.Session.GetString("login") != null)
            {
                ViewBag.Categories = databaseModel.ReturnContext().categories;
                return View();
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        public IActionResult CategoryCreate(Category category)
        {
            databaseModel.AddCategory(category);
            return RedirectToAction("Categories");
        }

        [HttpPost]
        public IActionResult CategoryDelete(int id)
        {
            databaseModel.DeleteCategory(databaseModel.ReturnCategoryById(id));
            return RedirectToAction("Categories");
        }

        public IActionResult Orders()
        {
            if (this.HttpContext.Session.GetString("login") != null)
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        public IActionResult Suppliers()
        {
            if (this.HttpContext.Session.GetString("login") != null)
            {
                ViewBag.Couriers = databaseModel.ReturnAllSuppliers();
                return View();
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        public IActionResult CourierCreate(Supplier courier)
        {
            databaseModel.AddSupplier(courier);
            return RedirectToAction("Suppliers");
        }

        public IActionResult CourierDelete(int id)
        {
            databaseModel.DeleteSupplier(databaseModel.ReturnSupplierByID(id));
            return RedirectToAction("Suppliers");
        }
    }
}
