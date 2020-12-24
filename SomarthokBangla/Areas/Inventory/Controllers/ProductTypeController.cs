using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SomarthokBangla.Data;
using SomarthokBangla.Models;

namespace SomarthokBangla.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class ProductTypeController : Controller
    {
        private ApplicationDbContext _db;

        public ProductTypeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var productCatList = _db.ProductTypes.Include(c => c.ProductCategory).ToList();
            return View(productCatList);
        }

        //Create Get Action Method

        [HttpGet]
        public ActionResult Create()
        {
            ViewData["SpecialTagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "TagName");
            return View();
        }

        //Create Post Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _db.ProductTypes.Add(productTypes);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product Category Added successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);

        }

        //Get Edit Action Method

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productTypes = _db.ProductTypes.Find(id);
            if (productTypes == null)
            {
                return NotFound();
            }
            return View(productTypes);
        }

        //Post Edit Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _db.Update(productTypes);
                await _db.SaveChangesAsync();
                TempData["edit"] = "Product Category updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);

        }

        //Get Details Action Method

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productTypes = _db.ProductTypes.Find(id);
            if (productTypes == null)
            {
                return NotFound();
            }
            return View(productTypes);
        }

        //Post Edit Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ProductTypes productTypes)
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
            var productTypes = _db.ProductTypes.Find(id);
            if (productTypes == null)
            {
                return NotFound();
            }
            return View(productTypes);
        }

        //Post Delete Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, ProductTypes productTypes)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id != productTypes.Id)
            {
                return NotFound();
            }

            var productType = _db.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Remove(productType);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product Category deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(productType);

        }
    }
}