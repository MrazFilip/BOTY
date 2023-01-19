using BOTY.Models;
using Microsoft.AspNetCore.Mvc;

namespace BOTY.Controllers
{
    public class CartController : BaseController
    {
        public MyContext context = new();
        
        public IActionResult Cart()
        {
            return View();
        }
    }
}
