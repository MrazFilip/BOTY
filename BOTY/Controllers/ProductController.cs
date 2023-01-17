using BOTY.Models;
using BOTY.Models.Tables;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BOTY.Controllers
{
    public class ProductController : BaseController
    {
        DatabaseModel databaseModel = new();
        public IActionResult Index(int id, int colorId = 0, int sizeId = 0)
        {
            var product = databaseModel.ReturnFullProductById(id);
            var images = databaseModel.ReturnImagesByProductId(id);
            var variants = databaseModel.ReturnVariantsByProductId(id);
            var colors = new List<Color>();
            var sizes = new List<Size>();
            foreach (var item in variants)
            {
                colors.Add(databaseModel.ReturnColorById(item.colorId));
            }
            ViewBag.Product = product;
            ViewBag.Colors = colors.Distinct().ToList();
            ViewBag.ImageCount = images.Count;
            ViewBag.Images = images;
            if (colorId == 0)
                colorId = colors[0].id;
            foreach (var item in variants)
            {
                if(item.colorId == colorId)
                    sizes.Add(databaseModel.ReturnSizeById(item.sizeId));
            }
            if (sizeId == 0)
                sizeId = sizes[0].Id;
            ViewBag.SelectedVariant = variants.Find(x => x.colorId == colorId && x.sizeId == sizeId);
            ViewBag.Id = id;
            ViewBag.SelectedColor = colorId;
            ViewBag.SelectedSize = sizeId;
            ViewBag.Sizes = sizes;
            string category = "";
            foreach (var item in product.categories)
            {
                category += item + ", ";
            }
            category = category.Substring(0, category.Length - 2);
            ViewBag.Categories = category;
            return View();
        }

        [HttpPost]
        public IActionResult AddToCard()
        {
            return RedirectToAction("Index");
        }
    }
}
