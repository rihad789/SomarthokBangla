using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomarthokBangla.Data;

namespace SomarthokBangla.Areas.Inventory.Controllers
{

    [Area("Inventory")]
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        //[Authorize(Roles = Role.Admin)]
        //[Authorize(AuthenticationSchemes = "Inventory")]
        public IActionResult Index()
        {
            var productCatList = _db.ProductCategory.ToList();

            return View(productCatList);
        }
    }
}

