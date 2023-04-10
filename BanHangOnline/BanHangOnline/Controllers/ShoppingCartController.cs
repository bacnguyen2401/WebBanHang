using BanHangOnline.Models;
using BanHangOnline.Models.EF;
using System;
using System.Collections.Generic;
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
            if(cart != null && cart.items.Any())
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
            if (cart != null && cart.items.Any() )
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
                return Json(new { Count = cart.items.Count } , JsonRequestBehavior.AllowGet);
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
            if(ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["cart"];
                if (cart != null && cart.items.Any() )
                {
                    Order order = new Order();
                    order.CustomerName = model.CustomerName;
                    order.Phone = model.Phone;
                    order.Address = model.Address;
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

                if (cart == null )
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
        public ActionResult Update(int id , int quantity)
        {
            ShoppingCart cart = (ShoppingCart)Session["cart"];
            if (cart != null && cart.items.Any())
            {
                cart.UpdateQuantity(id,quantity);
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
                if(checkProduct != null)
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
            if(cart != null && cart.items.Any())
            {
                cart.ClearCart();
                return Json(new {Success =  true});
            }
            return Json(new { Success = false });
        }
    }
}