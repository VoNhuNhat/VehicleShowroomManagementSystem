using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Showroom_Management_System.Areas.Admin.Data;

namespace Vehicle_Showroom_Management_System.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        Vehicle_Showroom_Management_SystemEntities db = new Vehicle_Showroom_Management_SystemEntities();
        // GET: Admin/Admin
        public ActionResult Index()
        {
            if (Session["username"] != null)
            {
                ViewBag.fullname = Session["fullname"];
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpGet]
        public ActionResult Login()
        {
            Session.Clear();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
            UserAccount userCheck = db.UserAccounts.Where(ua => ua.UserName == username && ua.Password == password).FirstOrDefault();
            if (userCheck != null)
            {
                Session["username"] = userCheck.UserName;
                Session["fullname"] = userCheck.FullName;
                return RedirectToAction("Index");
            }
                return View();
            }
                return View();
        }
    }
}