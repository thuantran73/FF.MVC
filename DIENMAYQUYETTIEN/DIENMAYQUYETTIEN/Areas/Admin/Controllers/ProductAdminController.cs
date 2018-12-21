using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DIENMAYQUYETTIEN.Models;
using System.Transactions;

namespace WebApplication.Controllers
{
    public class DanhSachSanPhamController : Controller
    {
        DIENMAYQUYETTIENEntities db = new DIENMAYQUYETTIENEntities();

        // GET: /DanhSachSanPham/
        public ActionResult Index()
        {
            var product = db.Products.Include(b => b.ProductType);
            return View(product.ToList());
        }

        // GET: /DanhSachSanPham/Details/5
        public FileResult Details(string id)
        {
            var path = Server.MapPath("~/App_Data");
            path = System.IO.Path.Combine(path, id);
            return File(path, "images");
        }

        // GET: /DanhSachSanPham/Create
        public ActionResult Create()
        {
            ViewBag.Prod_ID = new SelectList(db.ProductTypes, "ID", "ProductTypeName");
            return View();
        }

        // POST: /DanhSachSanPham/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product model)
        {
            CheckProducts(model);
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    db.Products.Add(model);
                    db.SaveChanges();

                    if (Request.Files["Picture"] != null &&
                        Request.Files["Picture"].ContentLength > 0)
                    {
                        var path = Server.MapPath("~/App_Data");
                        path = System.IO.Path.Combine(path, model.ID.ToString());
                        Request.Files["Picture"].SaveAs(path);

                        scope.Complete();
                        return RedirectToAction("Index");
                    }
                    else
                        ModelState.AddModelError("HinhAnh", "Chua chon hinh anh cho san pham");
                }
            }

            ViewBag.Prod_ID = new SelectList(db.ProductTypes, "id", "ProductTypeName", model.ProductTypeID);
            return View(model);
        }

        private void CheckProducts(Product model)
        {
            if (model.OriginPrice < 0)
                ModelState.AddModelError("GiaGoc", "Gia goc phai lon hon 0");
            if (model.OriginPrice > model.SalePrice)
                ModelState.AddModelError("GiaBan", "Gia ban phai lon hon gia goc");
            if (model.OriginPrice > model.InstallmentPrice)
                ModelState.AddModelError("GiaGop", "Gia gop phai lon hon gia goc");
            if (model.Quantity < 0)
                ModelState.AddModelError("SoLuongTon", "So luong ton phai lon hon 0");
        }

        // GET: /DanhSachSanPham/Edit/5
        public ActionResult Edit(int id)
        {
            Product model = db.Products.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.Loai_id = new SelectList(db.ProductTypes, "id", "TenLoai", model.ProductTypeID);
            return View(model);
        }

        // POST: /DanhSachSanPham/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            CheckProducts(model);
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();

                    var path = Server.MapPath("~/App_Data");
                    path = path + "/" + model.ID;
                    if (Request.Files["Picture"] != null &&
                        Request.Files["Picture"].ContentLength > 0)
                    {
                        Request.Files["Picture"].SaveAs(path);
                    }

                    scope.Complete(); // approve for transaction
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Loai_id = new SelectList(db.ProductTypes, "id", "ProductTypeName", model.ProductTypeID);
            return View(model);
        }

        // GET: /DanhSachSanPham/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product bangsanpham = db.Products.Find(id);
            if (bangsanpham == null)
            {
                return HttpNotFound();
            }
            return View(bangsanpham);
        }

        // POST: /DanhSachSanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product bangsanpham = db.Products.Find(id);
            db.Products.Remove(bangsanpham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
