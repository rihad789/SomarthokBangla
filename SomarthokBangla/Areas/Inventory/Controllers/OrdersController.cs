using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SomarthokBangla.Data;
using SomarthokBangla.Models;

namespace SomarthokBangla.Areas.Inventory.Controllers
{
    [Area(nameof(Inventory))]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var orderList = _db.Orders.ToList();
            return View(orderList);
        }

        public IActionResult ConfirmOrder(int id)
        {
            var user = _db.Orders.FirstOrDefault(c => c.Id == id);
            ViewData["userInfo"] = user;
            var orderDetails = _db.OrderDetails.Include(c=>c.Products).Where(c=>c.OrderId==id).ToList();
            ViewData["orderDetails"] = orderDetails;
            return View(orderDetails);
        }

        [HttpPost]
        [ActionName("ConfirmOrder")]
        public async Task<IActionResult> ConfirmOrderPost(int id)
        {

            var user = _db.Orders.FirstOrDefault(c => c.Id == id);
            user.OrderConfirmed = true;

                _db.Orders.Update(user);
                await _db.SaveChangesAsync();
                TempData["edit"] = "Product Updated successfully";
                return RedirectToAction(nameof(Index));
           
            //return View(orders);

            //var user = _db.Orders.FirstOrDefault(c => c.Id == id);
            //ViewData["userInfo"] = user;
            //var orderDetails = _db.OrderDetails.Include(c => c.Products).Where(c => c.OrderId == id).ToList();
            //ViewData["orderDetails"] = orderDetails;
            //return View(orderDetails);
        }
    }
}