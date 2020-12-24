using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomarthokBangla.Data;

namespace SomarthokBangla.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    //[Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }


        //[Authorize(AuthenticationSchemes = "Admin")]
        [Authorize]
        public IActionResult Index()
        {
            var productCatList = _db.ProductCategory.ToList();

            return View(productCatList);
        }
    }
}