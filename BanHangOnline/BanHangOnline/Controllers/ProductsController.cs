using BanHangOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        public ActionResult Index(int? id)
        {
            
            var items = _dbConect.Products.ToList();
            if (id != null)
            {
                items = items.Where(x => x.Id == id).ToList();
            }
            return View(items);
        }

        public ActionResult Detail(int id)
        {
            var item = _dbConect.Products.Find(id);
            if(item != null)
            {
                _dbConect.Products.Attach(item);
                item.VỉewCount = item.VỉewCount + 1;
                _dbConect.Entry(item).Property(x => x.VỉewCount).IsModified = true;
                _dbConect.SaveChanges();
            }
            return View(item);
        }

        public ActionResult ProductCategory(string alias , int? id)
        {

            var items = _dbConect.Products.ToList();
            if (id  > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate = _dbConect.ProductCategories.Find(id);
            if(cate != null)
            {
                ViewBag.CateName = cate.Title;
            }    
                
            ViewBag.CateId = id;
            return View(items);
        }

        public ActionResult Partial_ItemsByCateId()
        {
            var items = _dbConect.Products.Where(x => x.IsHome && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }

        public ActionResult Partial_ProductSales()
        {
            var items = _dbConect.Products.Where(x => x.IsHome && x.IsActive && x.IsSale).Take(12).ToList();
            return PartialView(items);
        }
    }
}