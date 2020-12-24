using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SomarthokBangla.Data;
using SomarthokBangla.Models;

namespace SomarthokBangla.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext _db;
        private IHostingEnvironment _he;

        public ProductsController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }
        public IActionResult Index()
        {
            var productList = _db.Products.Include(c=>c.ProductTypes).Include(c=>c.ProductTypes.ProductCategory).Include(c => c.ProductTypes.ProductCategory.SpecialTag).ToList();
            return View(productList);
        }

        //Setting Up Low and High Amount Filter
        //Post Index Action Method
        [HttpPost]
        public IActionResult Index(decimal? lowAmount, decimal? largeAmount)
        {
            var productList = _db.Products.Include(c => c.ProductTypes).Include(c => c.ProductTypes.ProductCategory).
                Include(c => c.ProductTypes.ProductCategory.SpecialTag).Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();
            //var product = _db.Products.Include(c => c.ProductCategory).Include(c => c.SpecialTag).
            //    Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();

            if (lowAmount == null || largeAmount == null)
            {
                productList = _db.Products.Include(c => c.ProductTypes).Include(c => c.ProductTypes.ProductCategory).
                    Include(c => c.ProductTypes.ProductCategory.SpecialTag).ToList();
            }

            return View(productList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            //ViewData["ProductTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            //ViewData["ProductCategoryId"] = new SelectList(_db.ProductCategory.ToList(), "Id", "CategoryName");
            ViewData["ManufacturerId"] = new SelectList(_db.Manufacturer.ToList(), "Id", "ManufacturerName");
            ViewData["SpecialTagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "TagName");
            return View();
        }

        [HttpGet]
        public IActionResult getCategoryList(int id)
        {
            List<ProductCategory> ProductCategoryList = new List<ProductCategory>();
            ProductCategoryList = _db.ProductCategory.Where(c=>c.SpecialTagId==id).ToList();
            return Json(new SelectList(ProductCategoryList, "Id", "CategoryName"));
        }

        [HttpGet]
        public IActionResult getProductTypeList(int id)
        {
            List<ProductTypes> ProductTypesList = new List<ProductTypes>();
            ProductTypesList = _db.ProductTypes.Where(c => c.ProductCategoryId == id).ToList();
            return Json(new SelectList(ProductTypesList, "Id", "ProductType"));
        }

        ////Create Post Action Method

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Products products)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        products.UplodaDate = DateTime.Now;
        //        _db.Products.Add(products);
        //        await _db.SaveChangesAsync();
        //        TempData["save"] = "Product Category Added successfully";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(products);

        //}

        //Post Create Method
        [HttpPost]
        public async Task<IActionResult> Create(Products product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                //Checking Product Name if Exist or Not
                var searchProduct = _db.Products.FirstOrDefault(c => c.Name == product.Name);
                if (searchProduct != null)
                {
                    ViewData["ProductCategoryID"] = new SelectList(_db.ProductCategory.ToList(), "Id", "CategoryName");
                    ViewData["SpecialTagId"] = new SelectList(_db.SpecialTag.ToList(), "Id", "TagName");
                    ViewBag.message = "This product Already Exist";
                    return View(product);
                }

                if (image != null)
                {
                    //var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    //await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    //product.Image = "Images/" + image.FileName;

                    var name = Path.Combine(_he.WebRootPath + "/Images", product.Name + Path.GetExtension(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "Images/" + product.Name + Path.GetExtension(image.FileName);
                }

                if (image == null)
                {
                    product.Image = "Images/NoImage.PNG";
                }
                product.UplodaDate = DateTime.Now;
                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product added successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        //Get Edit Action Method
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            ViewData["ManufacturerId"] = new SelectList(_db.Manufacturer.ToList(), "Id", "ManufacturerName");
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.ProductTypes.ProductCategory).
                Include(c => c.ProductTypes.ProductCategory.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //Post Edit Action Method
        [HttpPost]
        public async Task<IActionResult> Edit(Products product, IFormFile image)
        {
            if (ModelState.IsValid)
            {

                if (image != null)
                {
                    //var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    //await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    //product.Image = "Images/" + image.FileName;

                    var name = Path.Combine(_he.WebRootPath + "/Images", product.Name + Path.GetExtension(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "Images/" + product.Name+ Path.GetExtension(image.FileName);
                }

                if (image == null)
                {
                    product.Image = "Images/NoImage.PNG";
                }

                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                TempData["edit"] = "Product Updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        //Get Details Action Method
        [HttpGet]
        public ActionResult Details(int? id)
        {
            ViewData["ManufacturerId"] = new SelectList(_db.Manufacturer.ToList(), "Id", "ManufacturerName");

            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.ProductTypes.ProductCategory).
                Include(c => c.ProductTypes.ProductCategory.SpecialTag).Include(c=>c.Manufacturer).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //Get Delete Action Method
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.ProductTypes.ProductCategory).
                Include(c => c.ProductTypes.ProductCategory.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //Post Delete Action Method
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            TempData["delete"] = "Product Deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}