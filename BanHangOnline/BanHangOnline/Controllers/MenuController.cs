using BanHangOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuTop()
        {
            var item = _dbConect.Categories.OrderBy(x => x.Postision).ToList();
            return PartialView("_MenuTop" , item);
        }

        public ActionResult MenuProductCategory()
        {
            var items = _dbConect.ProductCategories.ToList();
            return PartialView("_MenuProductCategory", items);
        }

        public ActionResult MenuLeft(int? id)
        {
            if(id != null)
            {
                ViewBag.CateId = id;
            }
            var items = _dbConect.ProductCategories.ToList();
            return PartialView("_MenuLeft", items);
        }


        public ActionResult MenuArrivals()
        {
            var items = _dbConect.ProductCategories.ToList();
            return PartialView("_MenuArrivals", items);
        }
    }
}