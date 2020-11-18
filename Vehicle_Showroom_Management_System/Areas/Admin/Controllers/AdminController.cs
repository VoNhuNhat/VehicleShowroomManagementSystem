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
                ViewBag.car = db.Cars.ToList().Count();
                ViewBag.brand = db.Brands.ToList().Count();
                ViewBag.modelCar = db.ModelCars.ToList().Count();
                ViewBag.customer = db.Customers.ToList().Count();
                return View();
            }
            else
            {
                return RedirectToAction("Index","Login");
            }
        }
        [HttpGet]
        public ActionResult ProfileCurrentUser(int? userId)
        {
            int sessionUserId = Convert.ToInt32(Session["userId"]);
            if (userId == sessionUserId)
            {
                UserAccount userAccount = db.UserAccounts.Where(ua => ua.UserId == userId).FirstOrDefault();
                return View(userAccount);
            }
            else
            {
                UserAccount userAccount = db.UserAccounts.Where(ua => ua.UserId == sessionUserId).FirstOrDefault();
                return View(userAccount);
            }
             
        }
        
        [HttpGet]
        public ActionResult EditCurrentUser(int? userId)
        {
            int sessionUserId = Convert.ToInt32(Session["userId"]);
            if (userId == sessionUserId)
            {
            UserAccount userAccount = db.UserAccounts.Where(ua => ua.UserId == userId).FirstOrDefault();
            return View(userAccount);
            }
            else
            {
                UserAccount userAccount = db.UserAccounts.Where(ua => ua.UserId == sessionUserId).FirstOrDefault();
                return View(userAccount);
            }
        }
        [HttpPost]
        public ActionResult EditCurrentUser(int userId, string fullName, string userName, string address, string email, string phoneNumber, DateTime birthday)
        {
            UserAccount userAccount = db.UserAccounts.Where(ua => ua.UserId == userId).FirstOrDefault();
            userAccount.FullName = fullName;
            userAccount.UserName = userName;
            userAccount.Address = address;
            userAccount.Email = email;
            userAccount.PhoneNumber = phoneNumber;
            userAccount.FullName = fullName;
            userAccount.Birthday = birthday;
            db.SaveChanges();
            return RedirectToAction("ProfileCurrentUser", new {userId = userId });
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