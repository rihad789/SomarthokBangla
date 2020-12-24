using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SomarthokBangla.Data;
using SomarthokBangla.Models;

namespace SomarthokBangla.Areas.Customer.Controllers
{
    [Area(nameof(Customer))]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var productList = _db.Products.Include(c => c.ProductTypes).Include(c => c.ProductTypes.ProductCategory).Include(c => c.ProductTypes.ProductCategory.SpecialTag).ToList();

            return View(productList);
        }


    }
}
