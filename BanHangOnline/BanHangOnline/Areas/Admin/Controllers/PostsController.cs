using BanHangOnline.Models;
using BanHangOnline.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin ,Employee")]
    public class PostsController : Controller
    {
        // GET: Admin/Posts
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        public ActionResult Index()
        {
            var item = _dbConect.Posts.ToList();
            return View(item);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Posts model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.CategoryId = 7;
                model.Alias = BanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                _dbConect.Posts.Add(model);
                _dbConect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = _dbConect.Posts.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Posts model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedDate = DateTime.Now;
                model.Alias = BanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                _dbConect.Posts.Attach(model);
                _dbConect.Entry(model).State = System.Data.Entity.EntityState.Modified;
                _dbConect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbConect.Posts.Find(id);
            if (item != null)
            {
                _dbConect.Posts.Remove(item);
                _dbConect.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = _dbConect.Posts.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                _dbConect.Entry(item).State = System.Data.Entity.EntityState.Modified;
                _dbConect.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult deleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = _dbConect.Posts.Find(Convert.ToInt32(item));
                        _dbConect.Posts.Remove(obj);
                        _dbConect.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}