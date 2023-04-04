using BanHangOnline.Models;
using BanHangOnline.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    public class ProductCategoryController : Controller
    {
        // GET: Admin/ProductCategory
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        public ActionResult Index()
        {
            var items = _dbConect.ProductCategories.ToList();
            return View(items);
        }

        public ActionResult Add()
        {
            
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = BanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                _dbConect.ProductCategories.Add(model);
                _dbConect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();

        }

        public ActionResult Edit(int id)
        {
            var item = _dbConect.ProductCategories.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = BanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                _dbConect.Entry(model).State = System.Data.Entity.EntityState.Modified;
                _dbConect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbConect.ProductCategories.Find(id);
            if (item != null)
            {
                _dbConect.ProductCategories.Remove(item);
                _dbConect.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}