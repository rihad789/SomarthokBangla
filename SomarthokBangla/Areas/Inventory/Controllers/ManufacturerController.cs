using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SomarthokBangla.Data;
using SomarthokBangla.Models;

namespace SomarthokBangla.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Authorize]
    public class ManufacturerController : Controller
    {
        private ApplicationDbContext _db;

        public ManufacturerController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var productCatList = _db.Manufacturer.ToList();
            return View(productCatList);
        }

        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }

        //Create Post Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                _db.Manufacturer.Add(manufacturer);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product Category Added successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(manufacturer);

        }

        //Get Edit Action Method

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var manufacturer = _db.Manufacturer.Find(id);
            if (manufacturer == null)
            {
                return NotFound();
            }
            return View(manufacturer);
        }

        //Post Edit Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                _db.Update(manufacturer);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product Type updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(manufacturer);

        }

        //Get Details Action Method

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var manufacturer = _db.Manufacturer.Find(id);
            if (manufacturer == null)
            {
                return NotFound();
            }
            return View(manufacturer);
        }

        //Post Edit Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(Manufacturer manufacturer)
        {
            return RedirectToAction(nameof(Index));
        }

        //Get Delete Action Method

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var manufacturer = _db.Manufacturer.Find(id);
            if (manufacturer == null)
            {
                return NotFound();
            }
            return View(manufacturer);
        }

        //Post Delete Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, Manufacturer manufacturer)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id != manufacturer.Id)
            {
                return NotFound();
            }

            var manufacturers = _db.Manufacturer.Find(id);
            if (manufacturers == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Remove(manufacturers);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product Type deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(manufacturers);

        }


    }
}