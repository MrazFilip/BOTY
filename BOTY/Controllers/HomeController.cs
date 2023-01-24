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
            ViewBag.Count = products.Count;
            ViewBag.Rows = "1fr";

            return View();
        }

        public IActionResult Products(int manufacturerId = 0, int colorId = 0, int sizeId = 0, int categoryId = 0, int materialId = 0)
        {
            ViewBag.Manufacturer = manufacturerId;
            ViewBag.Color = colorId;
            ViewBag.Size = sizeId;
            ViewBag.Category = categoryId;
            ViewBag.Material = materialId;

            ViewBag.Manufacturers = databaseModel.ReturnContext().manufacturers.ToList();
            ViewBag.Colors = databaseModel.ReturnContext().colors.ToList();
            ViewBag.Sizes = databaseModel.ReturnContext().sizes.ToList();
            ViewBag.Categories = databaseModel.ReturnContext().categories.ToList();
            ViewBag.Materials = databaseModel.ReturnContext().materials.ToList();

            var products = new List<Product>();
            if (manufacturerId != 0 && materialId != 0)
            {
                products = databaseModel.ReturnContext().products.ToList().FindAll(x => x.manufacturer == manufacturerId && x.material == materialId);
            }
            else if(manufacturerId != 0)
            {
                products = databaseModel.ReturnContext().products.ToList().FindAll(x => x.manufacturer == manufacturerId);
            }
            else if (materialId != 0)
            {
                products = databaseModel.ReturnContext().products.ToList().FindAll(x => x.material == materialId);
            }
            else
            {
                products = databaseModel.ReturnContext().products.ToList();
            }
            
            var images = databaseModel.ReturnContext().images.ToList();
            var variations = databaseModel.ReturnContext().variants.ToList();
            var titleImage = new List<string>();
            var usedVariations = new List<Variant>();
            var toBeRemoved = new List<Product>();
            if (products.Count > 0)
            {
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
                            if (colorId != 0 && sizeId != 0)
                            {
                                if (colorId == variant.colorId && sizeId == variant.sizeId)
                                {
                                    usedVariations.Add(variant);
                                }
                                else
                                {
                                    toBeRemoved.Add(products.Find(x => x.Id == variant.productId));
                                }
                            }
                            else if (colorId != 0)
                            {
                                if (colorId == variant.colorId)
                                {
                                    usedVariations.Add(variant);  
                                }
                                else
                                {
                                    toBeRemoved.Add(products.Find(x => x.Id == variant.productId));
                                }
                            }
                            else if (sizeId != 0)
                            {
                                if (sizeId == variant.sizeId)
                                {
                                    usedVariations.Add(variant);  
                                }
                                else
                                {
                                    toBeRemoved.Add(products.Find(x => x.Id == variant.productId));
                                }
                            }
                            else
                            {
                                usedVariations.Add(variant);
                            }
                            break;
                        }
                    }
                }   
            }

            foreach (var item in toBeRemoved)
            {
                products.Remove(item);
            }
            ViewBag.Products = products;
            ViewBag.Variants = usedVariations;
            ViewBag.Images = titleImage;
            ViewBag.Count = products.Count;
            ViewBag.Rows = (products.Count / 4 + 1) + "fr";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
