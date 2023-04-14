using BanHangOnline.Models;
using BanHangOnline.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Partial_Subcrice()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Subcrice(Subscribe model)
        {
            if (ModelState.IsValid)
            {
                _context.Subscribes.Add(new Subscribe
                {
                    Email = model.Email,
                    CreatedDate = DateTime.Now
                });
                _context.SaveChanges();
                return Json(new { Success = true});
            }
            return View("Partial_Subcrice" , model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Refresh()
        {
            var item = new ThongKeModel();
            ViewBag.visitors_online = HttpContext.Application["visitors_online"];
            item.HomNay = HttpContext.Application["HomNay"].ToString();
            item.HomQua = HttpContext.Application["HomQua"].ToString();
            item.TuanNay = HttpContext.Application["TuanNay"].ToString();
            item.TuanTruoc = HttpContext.Application["TuanTruoc"].ToString();
            item.ThangTruoc = HttpContext.Application["ThangTruoc"].ToString();
            item.TatCa = HttpContext.Application["TatCa"].ToString();

            return PartialView(item);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}