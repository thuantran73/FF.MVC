using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DIENMAYQUYETTIEN.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Admin/Account
        public ActionResult Login()  
        {  
            return View();  
        }  
  
        [HttpPost]  
        [ValidateAntiForgeryToken]  
        public ActionResult Login(Models.Account objUser)   
        {  
            if (ModelState.IsValid)   
            {  
                using(Models.DIENMAYQUYETTIENEntities db = new Models.DIENMAYQUYETTIENEntities())  
                {  
                    var obj = db.Accounts.Where(a => a.Username.Equals(objUser.Username) && a.Password.Equals(objUser.Password)).FirstOrDefault();  
                    if (obj != null)  
                    {  
                        
                        Session["UserName"] = obj.FullName.ToString();  
                        return RedirectToAction("Index", "ProductAdmin");  
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                }  
            }  
            return View(objUser);  
        }  
  
        public ActionResult UserDashBoard()  
        {
            return View(); 
        }
        

    }

    
}