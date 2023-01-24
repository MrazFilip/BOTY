using System;
using BOTY.Models;
using BOTY.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BOTY.Controllers
{
    public class OrderController : BaseController
    {
        private DatabaseModel _databaseModel = new();
        
        [HttpGet]
        public IActionResult Index(int supplierId)
        {
            ViewBag.SupplierID = supplierId;
            return View(new FullOrder());
        }

        [HttpPost]
        public IActionResult Index(FullOrder order)
        {
            _databaseModel.CreateOrder(order);
            return RedirectToAction("Thanks");
        }

        public IActionResult Thanks()
        {
            var date = DateTime.Now;
            ViewBag.DateExpected = date.AddDays(3);
            return View();
        }
    }
}