using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Showroom_Management_System.Areas.Admin.Data;

namespace Vehicle_Showroom_Management_System.Areas.Admin.Controllers
{
    public class UserAccountController : Controller
    {
        Vehicle_Showroom_Management_SystemEntities db = new Vehicle_Showroom_Management_SystemEntities();
       [HttpGet]
        public ActionResult Index()
        {
            List<UserAccount> list = db.UserAccounts.ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string fullName, string userName, string password, string address, string email, string phoneNumber)
        {
            db.Insert_UserAccount(fullName, userName, password, address, email, phoneNumber);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult CheckUserName(string userName)
        {
            bool check = db.UserAccounts.ToList().Exists(u => u.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase));
            return Json(check);
        }
        [HttpPost]
        public JsonResult CheckEmail(string email)
        {
            bool check = db.UserAccounts.ToList().Exists(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
            return Json(check);
        }
        [HttpPost]
        public JsonResult CheckPhoneNumber(string phoneNumber)
        {
            bool check = db.UserAccounts.ToList().Exists(u => u.PhoneNumber.Equals(phoneNumber, StringComparison.CurrentCultureIgnoreCase));
            return Json(check);
        }
    }
}