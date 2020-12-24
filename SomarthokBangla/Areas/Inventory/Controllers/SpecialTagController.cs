using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SomarthokBangla.Models;
using SomarthokBangla.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SomarthokBangla.Areas.Inventory.Controllers
{

    [Area("Inventory")]
    public class SpecialTagController : Controller
    {
        private ApplicationDbContext _db;

        public SpecialTagController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var data = _db.SpecialTag.ToList();

            return View(data);
        }

        //Create Get Action Method

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //Create Post Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialTag specialTag)
        {
            if (ModelState.IsValid)
            {
                _db.SpecialTag.Add(specialTag);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product Type saved successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(specialTag);

        }

        //Get Edit Action Method

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _db.SpecialTag.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }

        //Post Edit Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SpecialTag specialTag)
        {
            if (ModelState.IsValid)
            {
                _db.Update(specialTag);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product Type updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(specialTag);

        }

        //Get Details Action Method

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _db.SpecialTag.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }

        //Post Edit Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(SpecialTag specialTag)
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
            var specialTag = _db.SpecialTag.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }

        //Post Delete Action Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, SpecialTag specialTags)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id != specialTags.Id)
            {
                return NotFound();
            }

            var specialTag = _db.SpecialTag.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Remove(specialTag);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product Type deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(specialTags);

        }
    }
}