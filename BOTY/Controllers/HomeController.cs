using BOTY.Models;
using BOTY.Models.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BOTY.Controllers
{
    public class HomeController : BaseController
    {
        private DatabaseModel databaseModel = new();

        public IActionResult Index()
        {
            var products = databaseModel.ReturnContext().products.ToList();
            var images = databaseModel.ReturnContext().images.ToList();
            var variations = databaseModel.ReturnContext().variants.ToList();
            var titleImage = new List<string>();
            var usedVariations = new List<Variant>();
            foreach (var item in products)
            {
                foreach(var image in images)
                {
                    if(image.productId == item.Id)
                    {
                        titleImage.Add(image.image);
                        break;
                    }
                }
                foreach (var variant in variations)
                {
                    if (variant.productId == item.Id)
                    {
                        usedVariations.Add(variant);
                        break;
                    }
                }
            }

            ViewBag.Products = products;
            ViewBag.Variants = usedVariations;
            ViewBag.Images = titleImage;
            if (products.Count < 8)
                ViewBag.Count = products.Count();
            else
                ViewBag.Count = 8;

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
