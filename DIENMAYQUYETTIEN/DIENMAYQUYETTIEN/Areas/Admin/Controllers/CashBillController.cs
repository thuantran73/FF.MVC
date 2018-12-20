using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DIENMAYQUYETTIEN.Models;


namespace DIENMAYQUYETTIEN.Areas.Admin.Controllers
{
    public class CashBillController : Controller
    {
        // GET: Admin/CashBill
        DIENMAYQUYETTIENEntities db = new DIENMAYQUYETTIENEntities();
        public ActionResult Index()
        {
            var cashbill = db.CashBills.OrderByDescending(x => x.ID).ToList();
            return View(cashbill);
        }

        // CREATE A NEW CASH BILL

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Prod = db.ProductTypes.OrderByDescending(x => x.ID).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(CashBill c)
        {
            var cash = new CashBill();
            cash.BillCode = c.BillCode;
            cash.CustomerName = c.CustomerName;
            cash.PhoneNumber = c.PhoneNumber;
            cash.Address = c.Address;
            cash.Date = c.Date;
            cash.Shipper = c.Shipper;
            cash.Note = c.Note;
            cash.GrandTotal = c.GrandTotal;
            
            db.CashBills.Add(cash);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            ViewBag.Prod = db.ProductTypes.OrderByDescending(x => x.ID).ToList();
            CashBill model = db.CashBills.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(CashBill c)
        {
            var cash = new CashBill();
            cash.BillCode = c.BillCode;
            cash.CustomerName = c.CustomerName;
            cash.PhoneNumber = c.PhoneNumber;
            cash.Address = c.Address;
            cash.Date = c.Date;
            cash.Shipper = c.Shipper;
            cash.Note = c.Note;
            cash.GrandTotal = c.GrandTotal;
            db.CashBills.Add(cash);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            ViewBag.Prod = db.ProductTypes.OrderByDescending(x => x.ID).ToList();

            CashBill model = db.CashBills.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: /CashBill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CashBill model = db.CashBills.Find(id);
            db.CashBills.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }

   
}