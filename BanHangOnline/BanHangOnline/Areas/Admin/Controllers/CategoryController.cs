using BanHangOnline.Models;
using BanHangOnline.Models.EF;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        // GET: Admin/Category
        public ActionResult Index()
        {
            var items = _dbConect.Categories;
            return View(items);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = BanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                _dbConect.Categories.Add(model);
                _dbConect.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = _dbConect.Categories.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                _dbConect.Categories.Attach(model);
                model.ModifiedDate = DateTime.Now;
                model.Alias = BanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                _dbConect.Entry(model).Property(x => x.Title).IsModified = true;
                _dbConect.Entry(model).Property(x => x.Description).IsModified = true;
                _dbConect.Entry(model).Property(x => x.Alias).IsModified = true;
                _dbConect.Entry(model).Property(x => x.SeoTitle).IsModified = true;
                _dbConect.Entry(model).Property(x => x.SeoDescription).IsModified = true;
                _dbConect.Entry(model).Property(x => x.SeoKeywords).IsModified = true;
                _dbConect.Entry(model).Property(x => x.Postision).IsModified = true;
                _dbConect.Entry(model).Property(x => x.ModifiedDate).IsModified = true;
                _dbConect.Entry(model).Property(x => x.ModifiedBy).IsModified = true;
                _dbConect.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbConect.Categories.Find(id);
            if (item != null)
            {
                //var DeleteItem = _dbConect.Categories.Attach(item);
                _dbConect.Categories.Remove(item);
                _dbConect.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}