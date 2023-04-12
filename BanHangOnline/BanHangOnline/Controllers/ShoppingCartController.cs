using BanHangOnline.Models;
using BanHangOnline.Models.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanHangOnline.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        ApplicationDbContext _dbConect = new ApplicationDbContext();
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["cart"];
            if (cart != null && cart.items.Any())
            {
                ViewBag.CkeckCart = cart;
            }
            return View();
        }

        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["cart"];
            if (cart != null && cart.items.Any())
            {
                ViewBag.CkeckCart = cart;
            }
            return View();
        }

        public ActionResult CheckOutSuccess()
        {
            return View();
        }

        public ActionResult Partial_Item_ThanhToan()
        {
            ShoppingCart cart = (ShoppingCart)Session["cart"];
            if (cart != null && cart.items.Any())
            {
                return PartialView(cart.items);
            }
            return PartialView();
        }

        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["cart"];
            if (cart != null && cart.items.Any())
            {
                return PartialView(cart.items);
            }
            return PartialView();
        }

        [HttpGet]
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["cart"];
            if (cart != null && cart.items.Any())
            {
                return Json(new { Count = cart.items.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Partial_CheckOut()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel model)
        {
            var code = new { Success = false, Code = -1 };
            if (ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["cart"];
                if (cart != null && cart.items.Any())
                {
                    Order order = new Order();
                    order.CustomerName = model.CustomerName;
                    order.Phone = model.Phone;
                    order.Address = model.Address;
                    order.Email = model.Email;
                    cart.items.ForEach(x => order.Details.Add(new OrderDetail
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Price = x.Price
                    }));
                    order.TotalAmount = cart.items.Sum(x => (x.Price * x.Quantity));
                    order.TypePayment = model.TypePayment;
                    Random random = new Random();
                    order.Code = "DH" + random.Next(0, 9) + random.Next(0, 9) + random.Next(0, 9) + random.Next(0, 9);
                    //order.Email = model.Email;
                    order.CreatedDate = DateTime.Now;
                    order.ModifiedDate = DateTime.Now;
                    order.CreatedBy = model.Phone;

                    _dbConect.Orders.Add(order);
                    _dbConect.SaveChanges();

                    // Send mail cho khánh hàng
                    var strSanPham = "";
                    var thanhTien = decimal.Zero;
                    var tongTien = decimal.Zero;
                    foreach (var sp in cart.items)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>" + sp.ProductName + "</td>";
                        strSanPham += "<td>" + sp.Quantity + "</td>";
                        strSanPham += "<td>" + BanHangOnline.Models.Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
                        strSanPham += "</tr>";

                        thanhTien += sp.Price * sp.Quantity;
                    }
                    tongTien = thanhTien;
                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                    contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", model.Email);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address);
                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}", BanHangOnline.Models.Common.Common.FormatNumber(thanhTien,0));
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", BanHangOnline.Models.Common.Common.FormatNumber(tongTien,0));
                    BanHangOnline.Models.Common.Common.SendMail("ShopOnline" , "Đơn hàng #" + order.Code , contentCustomer.ToString(), model.Email);

                    string contentCustomer1 = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                    contentCustomer1 = contentCustomer1.Replace("{{MaDon}}", order.Code);
                    contentCustomer1 = contentCustomer1.Replace("{{SanPham}}", strSanPham);
                    contentCustomer1 = contentCustomer1.Replace("{{TenKhachHang}}", order.CustomerName);
                    contentCustomer1 = contentCustomer1.Replace("{{Phone}}", order.Phone);
                    contentCustomer1 = contentCustomer1.Replace("{{Email}}", model.Email);
                    contentCustomer1 = contentCustomer1.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentCustomer1 = contentCustomer1.Replace("{{DiaChiNhanHang}}", order.Address);
                    contentCustomer1 = contentCustomer1.Replace("{{ThanhTien}}", BanHangOnline.Models.Common.Common.FormatNumber(thanhTien, 0));
                    contentCustomer1 = contentCustomer1.Replace("{{TongTien}}", BanHangOnline.Models.Common.Common.FormatNumber(tongTien, 0));
                    BanHangOnline.Models.Common.Common.SendMail("ShopOnline", "Đơn hàng mới  #" + order.Code, contentCustomer1.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);

                    cart.ClearCart();
                    return RedirectToAction("CheckOutSuccess");
                }
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new
            {
                Success = false,
                msg = "",
                code = -1,
                Count = 0
            };
            var checkProduct = _dbConect.Products.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["cart"];

                if (cart == null)
                {
                    cart = new ShoppingCart();
                }

                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Quantity = quantity,
                    Alias = checkProduct.Alias,
                };
                if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
                }
                item.Price = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    item.Price = (decimal)checkProduct.PriceSale;
                }
                item.TotalPrice = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                Session["cart"] = cart;
                code = new
                {
                    Success = true,
                    msg = "Thêm sản phẩm vào giỏ hàng thành công",
                    code = 1,
                    Count = cart.items.Count
                };
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            ShoppingCart cart = (ShoppingCart)Session["cart"];
            if (cart != null && cart.items.Any())
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new
            {
                Success = false,
                msg = "",
                code = -1,
                Count = 0
            };

            ShoppingCart cart = (ShoppingCart)Session["cart"];

            if (cart != null && cart.items.Any())
            {
                var checkProduct = cart.items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    code = new
                    {
                        Success = true,
                        msg = "Xóa thành công",
                        code = 1,
                        Count = cart.items.Count
                    };
                }
            }

            return Json(code);
        }

        [HttpPost]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = (ShoppingCart)Session["cart"];
            if (cart != null && cart.items.Any())
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
    }
}