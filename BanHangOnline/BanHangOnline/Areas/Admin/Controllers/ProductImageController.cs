using BanHangOnline.Models;
using BanHangOnline.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    public class ProductImageController : Controller
    {
        // GET: Admin/ProductImage
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        public ActionResult Index(int Id)
        {
            ViewBag.ProductId = Id;
            var items = _dbConect.ProductImages.Where(x => x.ProductId == Id).ToList();
            return View(items);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbConect.ProductImages.Find(id);
            _dbConect.ProductImages.Remove(item);
            _dbConect.SaveChanges();
            return Json(new {success = true});
        }
        [HttpPost]
        public ActionResult AddImage(int productId, string url)
        {
            _dbConect.ProductImages.Add(new ProductImage()
            {
                ProductId = productId,
                Image = url,
                IsDefault = false
            });

            _dbConect.SaveChanges();
            return Json(new { success = true });
        }
    }
}