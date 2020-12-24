using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SomarthokBangla.Models;
using SomarthokBangla.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace SomarthokBangla.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Authorize]
    public class ProductCategoryController : Controller
    {
        private ApplicationDbContext _db;

        public ProductCategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var productCatList = _db.ProductCategory.Include(c => c.SpecialTag).ToList();
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
        public async Task<IActionResult> Create(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                _db.ProductCategory.Add(productCategory);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product Category Added successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(productCategory);

        }

        //Get Edit Action Method

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            ViewData["SpecialTagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "TagName");
            if (id == null)
            {
                return NotFound();
            }
            var productCategory = _db.ProductCategory.Find(id);
            if (productCategory == null)
            {
                return NotFound();
            }
            return View(productCategory);
        }

        //Post Edit Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                _db.Update(productCategory);
                await _db.SaveChangesAsync();
                TempData["edit"] = "Product Category updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(productCategory);

        }

        //Get Details Action Method

        [HttpGet]
        public ActionResult Details(int? id)
        {
            ViewData["SpecialTagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "TagName");

            if (id == null)
            {
                return NotFound();
            }
            var productCategory = _db.ProductCategory.Find(id);
            if (productCategory == null)
            {
                return NotFound();
            }
            return View(productCategory);
        }

        //Post Edit Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ProductCategory productCategory)
        {
            return RedirectToAction(nameof(Index));
        }

        //Get Delete Action Method

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            ViewData["SpecialTagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "TagName");
            if (id == null)
            {
                return NotFound();
            }
            var productCategory = _db.ProductCategory.Find(id);
            if (productCategory == null)
            {
                return NotFound();
            }
            return View(productCategory);
        }

        //Post Delete Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, ProductCategory productcategory)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id != productcategory.Id)
            {
                return NotFound();
            }

            var productCategory = _db.ProductCategory.Find(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Remove(productCategory);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product Category deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(productCategory);

        }
    }
}