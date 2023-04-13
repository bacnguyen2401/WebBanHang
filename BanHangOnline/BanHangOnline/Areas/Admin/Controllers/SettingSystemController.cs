using BanHangOnline.Models;
using BanHangOnline.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    public class SettingSystemController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/SettingSystem
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Partial_Setting()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddSetting(SettingSystemViewModel model)
        {
            SystemSetting set = null;
            // Title
            var checkTitle = db.SystemSettings.FirstOrDefault(x => x.SettingKet.Contains("SettingTitle"));
            if (checkTitle == null || string.IsNullOrEmpty(checkTitle.SettingValue))
            {
                set = new SystemSetting();
                set.SettingKet = "SettingTitle";
                set.SettingValue = model.SettingTitle;
                db.SystemSettings.Add(set);
            }
            else
            {
                checkTitle.SettingValue = model.SettingTitle;
                db.Entry(checkTitle).State = System.Data.Entity.EntityState.Modified;
            }

            // Logo
            var checkLogo = db.SystemSettings.FirstOrDefault(x => x.SettingKet.Contains("SettingLogo"));
            if (checkLogo == null || string.IsNullOrEmpty(checkLogo.SettingValue))
            {
                set = new SystemSetting();
                set.SettingKet = "SettingLogo";
                set.SettingValue = model.SettingLogo;
                db.SystemSettings.Add(set);
            }
            else
            {
                checkLogo.SettingValue = model.SettingLogo;
                db.Entry(checkLogo).State = System.Data.Entity.EntityState.Modified;
            }

            // Hotline
            var checkHotline = db.SystemSettings.FirstOrDefault(x => x.SettingKet.Contains("SettingHotline"));
            if (checkHotline == null || string.IsNullOrEmpty(checkHotline.SettingValue))
            {
                set = new SystemSetting();
                set.SettingKet = "SettingHotline";
                set.SettingValue = model.SettingHotline;
                db.SystemSettings.Add(set);
            }
            else
            {
                checkHotline.SettingValue = model.SettingHotline;
                db.Entry(checkHotline).State = System.Data.Entity.EntityState.Modified;
            }

            // Email
            var checkEmail = db.SystemSettings.FirstOrDefault(x => x.SettingKet.Contains("SettingEmail"));
            if (checkEmail == null || string.IsNullOrEmpty(checkEmail.SettingValue))
            {
                set = new SystemSetting();
                set.SettingKet = "SettingEmail";
                set.SettingValue = model.SettingEmail;
                db.SystemSettings.Add(set);
            }
            else
            {
                checkEmail.SettingValue = model.SettingEmail;
                db.Entry(checkEmail).State = System.Data.Entity.EntityState.Modified;
            }

            // SettingTitleSeo
            var checkTitleSeo = db.SystemSettings.FirstOrDefault(x => x.SettingKet.Contains("SettingTitleSeo"));
            if (checkTitleSeo == null || string.IsNullOrEmpty(checkTitleSeo.SettingValue))
            {
                set = new SystemSetting();
                set.SettingKet = "SettingTitleSeo";
                set.SettingValue = model.SettingTitleSeo;
                db.SystemSettings.Add(set);
            }
            else
            {
                checkTitleSeo.SettingValue = model.SettingTitleSeo;
                db.Entry(checkTitleSeo).State = System.Data.Entity.EntityState.Modified;
            }

            // SettingDesSeo
            var checkDesSeo = db.SystemSettings.FirstOrDefault(x => x.SettingKet.Contains("SettingDesSeo"));
            if (checkDesSeo == null || string.IsNullOrEmpty(checkDesSeo.SettingValue))
            {
                set = new SystemSetting();
                set.SettingKet = "SettingDesSeo";
                set.SettingValue = model.SettingDesSeo;
                db.SystemSettings.Add(set);
            }
            else
            {
                checkDesSeo.SettingValue = model.SettingDesSeo;
                db.Entry(checkDesSeo).State = System.Data.Entity.EntityState.Modified;
            }

            // SettingKeySeo
            var checkKeySeo = db.SystemSettings.FirstOrDefault(x => x.SettingKet.Contains("SettingKeySeo"));
            if (checkKeySeo == null || string.IsNullOrEmpty(checkKeySeo.SettingValue))
            {
                set = new SystemSetting();
                set.SettingKet = "SettingKeySeo";
                set.SettingValue = model.SettingKeySeo;
                db.SystemSettings.Add(set);
            }
            else
            {
                checkKeySeo.SettingValue = model.SettingKeySeo;
                db.Entry(checkKeySeo).State = System.Data.Entity.EntityState.Modified;
            }

            db.SaveChanges();

            return View("Partial_Setting");
        }

    }
}