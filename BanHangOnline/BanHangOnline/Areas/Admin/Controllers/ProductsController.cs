using BanHangOnline.Models;
using BanHangOnline.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Admin/Products
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        public ActionResult Index(int? page)
        {
            IEnumerable<Product> items = _dbConect.Products.OrderByDescending(x => x.Id);
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            View(items).ViewBag.PageSize = pageSize;
            View(items).ViewBag.Page = page;
            return View(items);
        }

        public ActionResult Add()
        {
            ViewBag.ProductCategory = new SelectList(_dbConect.ProductCategories.ToList(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product model, List<string> Images, List<int> rDefault)
        {
            if (ModelState.IsValid)
            {
                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        if (i + 1 == rDefault[0])
                        {
                            model.Image = Images[i];
                            model.ProductImage.Add(new ProductImage
                            {
                                ProductId = model.Id,
                                Image = Images[i],
                                IsDefault = true
                            });
                        }
                        else
                        {
                            model.ProductImage.Add(new ProductImage
                            {
                                ProductId = model.Id,
                                Image = Images[i],
                                IsDefault = false
                            });
                        }
                    }
                }
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                if (string.IsNullOrEmpty(model.SeoTitle))
                {
                    model.SeoTitle = model.Title;
                }
                if (string.IsNullOrEmpty(model.Alias))
                {
                    model.Alias = BanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                }
                _dbConect.Products.Add(model);
                _dbConect.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategory = new SelectList(_dbConect.ProductCategories.ToList(), "Id", "Title");
            return View(model);
        }

        public ActionResult Edit(int id)
        {

            ViewBag.ProductCategory = new SelectList(_dbConect.ProductCategories.ToList(), "Id", "Title");
            var items = _dbConect.Products.Find(id);
            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedDate = DateTime.Now;
                model.CreatedDate = DateTime.Now;
                if (string.IsNullOrEmpty(model.SeoTitle))
                {
                    model.SeoTitle = model.Title;
                }
                if (string.IsNullOrEmpty(model.Alias))
                {
                    model.Alias = BanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                }
                _dbConect.Products.Attach(model);
                _dbConect.Entry(model).State = System.Data.Entity.EntityState.Modified;
                _dbConect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbConect.Products.Find(id);
            if (item != null)
            {
                _dbConect.Products.Remove(item);
                _dbConect.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = _dbConect.Products.Find(id);
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
        public ActionResult IsActiveHome(int id)
        {
            var item = _dbConect.Products.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                _dbConect.Entry(item).State = System.Data.Entity.EntityState.Modified;
                _dbConect.SaveChanges();
                return Json(new { success = true, isHome = item.IsHome });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsActiveSale(int id)
        {
            var item = _dbConect.Products.Find(id);
            if (item != null)
            {
                item.IsSale = !item.IsSale;
                _dbConect.Entry(item).State = System.Data.Entity.EntityState.Modified;
                _dbConect.SaveChanges();
                return Json(new { success = true, isSale = item.IsSale });
            }
            return Json(new { success = false });
        }
    }
}