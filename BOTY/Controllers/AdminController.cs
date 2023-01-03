using BOTY.Models;
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
            ViewBag.Materials = databaseModel.ReturnContext().materials;
            ViewBag.Manufacturers = databaseModel.ReturnContext().manufacturers;
            Product product = new Product();
            return View(product);
        }

        [HttpPost]
        public IActionResult ShoeCreate(Product product)
        {
            databaseModel.AddProduct(product);
            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult ShoeEdit(int id)
        {
            ViewBag.Materials = databaseModel.ReturnContext().materials;
            ViewBag.Manufacturers = databaseModel.ReturnContext().manufacturers;
            Product product = databaseModel.ReturnProductById(id);
            return View(product);
        }

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
            ViewBag.Materials = databaseModel.ReturnContext().materials;
            return View();
        }

        [HttpGet]
        public IActionResult MaterialCreate()
        {
            Material product = new();
            return View(product);
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
            Material material = databaseModel.ReturnMaterialById(id);
            return View(material);
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
            ViewBag.Manufacturers = databaseModel.ReturnContext().manufacturers;
            return View();
        }

        [HttpGet]
        public IActionResult ManufacturerCreate()
        {
            Manufacturer manufacturer = new();
            return View(manufacturer);
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
            Manufacturer manufacturer = databaseModel.ReturnManufacturerById(id);
            return View(manufacturer);
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
            ViewBag.ID = id;
            ViewBag.Variants = databaseModel.ReturnFullVariants(id);
            return View();
        }

        [HttpGet]
        public IActionResult VariantCreate(int productId)
        {
            ViewBag.ID = productId;
            ViewBag.Colors = databaseModel.ReturnContext().colors;
            ViewBag.Sizes = databaseModel.ReturnContext().sizes;
            Variant variant = new Variant();
            return View(variant);
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
            this.productId = productId;
            ViewBag.Variant = databaseModel.ReturnFullVariants(variantId);
            return View();
        }

        [HttpPost]
        public IActionResult VariantEdit(Variant variant)
        {
            databaseModel.UpdateVariant(variant);
            return RedirectToAction("Variant", new { id = this.productId });
        }

        public IActionResult VariantDelete(int id, int productId)
        {
            databaseModel.DeleteVariant(databaseModel.ReturnVariantByVariantId(id));
            return RedirectToAction("Variant", new { id = productId });
        }

        public IActionResult Colors()
        {
            ViewBag.Colors = databaseModel.ReturnContext().colors;
            return View();
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
            ViewBag.Sizes = databaseModel.ReturnContext().sizes;
            return View();
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

        public IActionResult Orders()
        {
            return View();
        }

        public IActionResult Suppliers()
        {
            return View();
        }
    }
}
