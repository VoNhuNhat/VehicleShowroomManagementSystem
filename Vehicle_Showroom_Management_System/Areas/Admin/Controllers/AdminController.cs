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
        bool logined = false;
        public bool checkLogin()
        {
            if (Session["username"] != null)    
            {
                logined = true;
            }
            else
            {
                logined = false;
            }
            return logined;
        }

        public ActionResult Index()
        {
            if (checkLogin())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index","Login");
            }
        }
        [HttpGet]
        public ActionResult Profile(int userId)
        {
            UserAccount userAccount = db.UserAccounts.Where(ua => ua.UserId == userId).FirstOrDefault();
            return View(userAccount);
        }
        
        [HttpGet]
        public ActionResult EditCurrentUser(int userId)
        {
            UserAccount userAccount = db.UserAccounts.Where(ua => ua.UserId == userId).FirstOrDefault();
            UserAccountController uac = new UserAccountController();
            userAccount.Password = uac.DecryptPassword(userAccount.Password);
            return View(userAccount);
        }

       [HttpPost]
        public JsonResult ResetPasswordInAdminPage(int userId, string current_password, string password)
        {
            bool check;
            UserAccount userAccount = db.UserAccounts.Where(ua => ua.UserId == userId).FirstOrDefault();
            if (userAccount != null)
            {
                UserAccountController uac = new UserAccountController();
                string userPasswordDecrypt = uac.DecryptPassword(userAccount.Password);
                if (current_password == userPasswordDecrypt)
                {
                string passwordEncrypt = uac.EncryptPassword(password);
                userAccount.Password = passwordEncrypt;
                db.SaveChanges();
                check = true;
                }
                else
                {
                 check = false;
                }
            }
            else
            {
                 check = false;
            }
            return Json(check);
        }

    }
}