using System.Collections.Generic;
using BOTY.Models;
using BOTY.Models.Tables;
using Microsoft.AspNetCore.Mvc;

namespace BOTY.Controllers
{
    public class CartController : BaseController
    {
        private DatabaseModel _databaseModel = new();
        
        public IActionResult Index(int supplierId = 1)
        {
            var products = new List<Product>();
            var colors = new List<Color>();
            var sizes = new List<Size>();
            var images = new List<Image>();
            double total = 0;
            var subtotals = new List<double>();
            double subtotal = 0;
            int index = 0;
            foreach (var item in SessionCart.cart)
            {
                products.Add(_databaseModel.ReturnProductById(item.productId));
                colors.Add(_databaseModel.ReturnColorById(item.colorId));
                sizes.Add(_databaseModel.ReturnSizeById(item.sizeId));
                images.Add(_databaseModel.ReturnImagesByProductId(item.productId)[0]);
                if (item.sale == 0)
                {
                    total += item.price;
                    subtotals.Add(item.price * SessionCart.count[index]);
                    subtotal += item.price * SessionCart.count[index];
                }
                else
                {
                    total += item.sale;
                    subtotals.Add(item.sale * SessionCart.count[index]);
                    subtotal += item.sale * SessionCart.count[index];
                }

                index++;
            }
            
            ViewBag.Variants = SessionCart.cart;
            ViewBag.Products = products;
            ViewBag.Colors = colors;
            ViewBag.Sizes = sizes;
            ViewBag.Images = images;
            ViewBag.Total = total;
            ViewBag.Count = SessionCart.cart.Count;
            ViewBag.Subtotals = subtotals;
            ViewBag.Quantity = SessionCart.count;
            ViewBag.Subtotal = subtotal;
            subtotal += _databaseModel.ReturnSupplierByID(supplierId).price;
            ViewBag.Total = subtotal;
            ViewBag.Suppliers = _databaseModel.ReturnAllSuppliers();
            SessionCart.supplierId = supplierId;
            ViewBag.SupplierID = supplierId;

            return View();
        }
        public IActionResult AddQuantity(int index, int stock)
        {
            if (SessionCart.count[index] < stock)
                SessionCart.count[index]++;
            return RedirectToAction("Index");
        }

        public IActionResult DecreaseQuantity(int index)
        {
            SessionCart.count[index]--;
            if (SessionCart.count[index] == 0)
            {
                SessionCart.cart.RemoveAt(index);
                SessionCart.count.RemoveAt(index);
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveItem(int index)
        {
            SessionCart.cart.RemoveAt(index);
            SessionCart.count.RemoveAt(index);
            return RedirectToAction("Index");
        }
    }
}
