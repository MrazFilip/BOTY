using BOTY.Models;
using BOTY.Models.Tables;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BOTY.Controllers
{
    public class ProductController : BaseController
    {
        DatabaseModel databaseModel = new();
        public IActionResult Index(int id)
        {
            ViewBag.Product = databaseModel.ReturnProductById(id);
            var images = databaseModel.ReturnImagesByProductId(id);
            var variants = databaseModel.ReturnVariantsByProductId(id);
            var colors = new List<Color>();
            var sizes = new List<Size>();
            foreach (var item in variants)
            {
                colors.Add(databaseModel.ReturnColorById(item.colorId));
                sizes.Add(databaseModel.ReturnSizeById(item.sizeId));
            }
            ViewBag.Variants = variants;
            ViewBag.Colors = colors;
            ViewBag.Sizes = sizes;
            ViewBag.ImageCount = images.Count;
            ViewBag.Images = images;
            return View();
        }
    }
}
