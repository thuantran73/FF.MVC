using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DIENMAYQUYETTIEN.Models;
using System.Net;
using System.Activities.Statements;
using System.Transactions;
using TransactionScope = System.Transactions.TransactionScope;

namespace DIENMAYQUYETTIEN.Areas.Admin.Controllers
{
    public class ProductAdminController : Controller
    {
        DIENMAYQUYETTIENEntities db = new DIENMAYQUYETTIENEntities();
        //
        // GET: /Admin/ProductAdmin/
        public ActionResult Index(){
            var product = db.Products.OrderByDescending(x => x.ID).ToList();
            return View(product);
        }
        
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Prod = db.ProductTypes.OrderByDescending(x => x.ID).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product p)
        {
            using (var scope = new TransactionScope())
            {
                var pro = new Product();
                pro.ProductCode = p.ProductCode;
                pro.ProductName = p.ProductName;
                pro.SalePrice = p.SalePrice;
                pro.OriginPrice = p.OriginPrice;
                pro.InstallmentPrice = p.InstallmentPrice;
                pro.Quantity = p.Quantity;
                pro.Status = p.Status;
                pro.ProductTypeID = p.ProductTypeID;
                db.Products.Add(pro);
                db.SaveChanges();

                var path = Server.MapPath("~/App_Data");
                path = path + "/" + p.ID;
                if (Request.Files["HinhAnh"] != null &&
                    Request.Files["HinhAnh"].ContentLength > 0)
                {
                    Request.Files["HinhAnh"].SaveAs(path);

                    scope.Complete(); // approve for transaction
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError("HinhAnh", "Chưa có hình sản phẩm!");
            }
            

            return RedirectToAction("Index");
        }

        
        
        public ActionResult Edit(int id)
        {
            ViewBag.Prod = db.ProductTypes.OrderByDescending(x => x.ID).ToList();
            Product model = db.Products.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Product p)
        {
            var pro = new Product();
            pro.ProductCode = p.ProductCode;
            pro.ProductName = p.ProductName;
            pro.SalePrice = p.SalePrice;
            pro.OriginPrice = p.OriginPrice;
            pro.InstallmentPrice = p.InstallmentPrice;
            pro.Quantity = p.Quantity;
            pro.Status = p.Status;
            pro.ProductTypeID = p.ProductTypeID;
            db.Products.Add(pro);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            ViewBag.Prod = db.ProductTypes.OrderByDescending(x => x.ID).ToList();
            
            Product model = db.Products.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: /BangSanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product model = db.Products.Find(id);
            db.Products.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}