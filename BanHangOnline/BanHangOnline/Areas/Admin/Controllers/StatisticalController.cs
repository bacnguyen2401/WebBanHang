﻿using BanHangOnline.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatisticalController : Controller
    {
        // GET: Admin/Statistical
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetStatistical(string fromDate ,string toDate)
        {
            var query = from o in db.Orders
                        join od in db.OrderDetails
                        on o.Id equals od.OrderId
                        join p in db.Products
                        on od.ProductId equals p.Id
                        select new
                        {
                            CreateDate = o.CreatedDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginPrice = p.OriginalPrice
                        };
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreateDate >= startDate);
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime startEnd = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreateDate <= startEnd);
            }

            var result = query.GroupBy(x => DbFunctions.TruncateTime(x.CreateDate)).Select(x => new
            {
                Date = x.Key.Value,
                TotalBuy = x.Sum(y => y.Quantity * y.OriginPrice),
                TotalSell = x.Sum(y => y.Quantity * y.Price)
            }).Select(x => new
            {
                Date = x.Date,
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy
            });

            return Json(new {Data = result } ,JsonRequestBehavior.AllowGet);
        }
    }
}