using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SomarthokBangla.Data;
using SomarthokBangla.Models;
using SomarthokBangla.Utility;

namespace SomarthokBangla.Areas.Customer.Controllers
{
    [Area(nameof(Customer))]
    public class ProductsController : Controller
    {
        private ApplicationDbContext _db;


        public ProductsController(ApplicationDbContext db)
        {
            _db = db;

        }
        public IActionResult Index()
        {
            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.ProductTypes.ProductCategory).
                   Include(c => c.ProductTypes.ProductCategory.SpecialTag).ToList();
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Type(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.ProductTypes.ProductCategory).
                Include(c => c.ProductTypes.ProductCategory.SpecialTag).Include(c => c.Manufacturer).Where(c => c.ProductTypeId == id).ToList();
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }



        public IActionResult Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.ProductTypes.ProductCategory).
                Include(c => c.ProductTypes.ProductCategory.SpecialTag).Include(c => c.Manufacturer).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var TypeId = product.ProductTypes.Id;
            var specialId = product.ProductTypes.ProductCategory.SpecialTagId;


            ViewData["similarProduct"] = _db.Products.Include(c => c.ProductTypes).Include(c => c.ProductTypes.ProductCategory).
                   Include(c => c.ProductTypes.ProductCategory.SpecialTag).Where(c=>c.ProductTypeId==TypeId).ToList();

            ViewData["similarCategory"] = _db.Products.Include(c => c.ProductTypes).Include(c => c.ProductTypes.ProductCategory).
                  Include(c => c.ProductTypes.ProductCategory.SpecialTag).Where(c=>c.ProductTypes.ProductCategory.SpecialTag.Id==specialId).ToList();

            return View(product);
        }

        //Add To Cart Post product Details Action Method
        [HttpPost]
        [ActionName("Details")]
        public ActionResult ProductDetails(int? id)
        {
            List<Products> productsList = new List<Products>();
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.ProductTypes.ProductCategory).
                Include(c => c.ProductTypes.ProductCategory.SpecialTag).Include(c => c.Manufacturer).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            productsList = HttpContext.Session.Get<List<Products>>("productsList");
            if (productsList == null)
            {
                productsList = new List<Products>();
            }
            var TypeId = product.ProductTypes.Id;
            var specialId = product.ProductTypes.ProductCategory.SpecialTagId;


            ViewData["similarProduct"] = _db.Products.Include(c => c.ProductTypes).Include(c => c.ProductTypes.ProductCategory).
                   Include(c => c.ProductTypes.ProductCategory.SpecialTag).Where(c => c.ProductTypeId == TypeId).ToList();

            ViewData["similarCategory"] = _db.Products.Include(c => c.ProductTypes).Include(c => c.ProductTypes.ProductCategory).
                  Include(c => c.ProductTypes.ProductCategory.SpecialTag).Where(c => c.ProductTypes.ProductCategory.SpecialTag.Id == specialId).ToList();

            productsList.Add(product);
            HttpContext.Session.Set("productsList", productsList);
            return View(product);
        }

        //Get Remove Cart Action Method
        [HttpGet]
        [ActionName("Remove")]
        public IActionResult RemoveFromCart(int? id)
        {
            List<Products> productsList = HttpContext.Session.Get<List<Products>>("productsList");
            if (productsList != null)
            {
                var product = productsList.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    productsList.Remove(product);
                    HttpContext.Session.Set("productsList", productsList);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        //Remove From Cart Post product Details Action Method
        [HttpPost]
        public IActionResult Remove(int? id)
        {
            List<Products> productsList = HttpContext.Session.Get<List<Products>>("productsList");
            if (productsList != null)
            {
                var product = productsList.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    productsList.Remove(product);
                    HttpContext.Session.Set("productsList", productsList);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        //Get product Cart Action Method
        public IActionResult Cart()
        {
            List<Products> productsList = HttpContext.Session.Get<List<Products>>("productsList");
            if (productsList == null)
            {
                productsList = new List<Products>();
            }
            return View(productsList);
        }

        //Get CheckOut Action Method
        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        //Post CheckOut Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Orders order)
        {
            List<Products> productsList = HttpContext.Session.Get<List<Products>>("productsList");
            if (productsList != null)
            {
                foreach (var item in productsList)
                {
                    OrderDetails orderDetails = new OrderDetails
                    {
                        ProductId = item.Id
                    };
                    order.OrderDetails.Add(orderDetails);
                }

            }
            order.OrderNo = GetOrderNo();
            order.OrderDate = DateTime.Now;
            _db.Orders.Add(order);

            var orderPlaced = await _db.SaveChangesAsync();

            TempData["order"] = "Order Placed successfully";
            HttpContext.Session.Set("productsList", new List<Products>());
            return RedirectToAction(nameof(Index));
        }

        public string GetOrderNo()
        {
            int rowCount = _db.Orders.ToList().Count + 1;
            return rowCount.ToString("000");
        }

        //Get Search Action Method
        [HttpGet]
        public async Task<IActionResult> Search(string searchString)
        {
            ViewData["searchString"] = searchString;
            var product = from m in _db.Products
                          select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                product = product.Where(s => s.Name.Contains(searchString));
            }

            return View(await product.ToListAsync());


        }

    }
}