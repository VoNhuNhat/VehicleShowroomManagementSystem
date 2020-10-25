using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Showroom_Management_System.Areas.Admin.Data;
using Vehicle_Showroom_Management_System.Areas.Admin.Controllers;
using System.Security.Cryptography.X509Certificates;

namespace Vehicle_Showroom_Management_System.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        Vehicle_Showroom_Management_SystemEntities db = new Vehicle_Showroom_Management_SystemEntities();
        [HttpGet]
        public ActionResult Index()
        {
            Session.Clear();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string username, string password)
        {
            if (ModelState.IsValid)
            {
                UserAccountController uac = new UserAccountController();
                string ePassword = uac.EncryptPassword(password);
                UserAccount userCheck = db.UserAccounts.Where(ua => ua.UserName == username && ua.Password == ePassword).FirstOrDefault();
                if (userCheck != null)
                {
                    Session["username"] = userCheck.UserName;
                    Session["fullname"] = userCheck.FullName;
                    return RedirectToAction("Index","Admin");
                }
                return View();
            }
            return View();
        }

        [HttpPost]
        public JsonResult CheckLogin(string username, string password)
        {
            UserAccountController uac = new UserAccountController();
            string ePassword = uac.EncryptPassword(password);
            UserAccount userCheck = db.UserAccounts.Where(ua => ua.UserName == username && ua.Password == ePassword).FirstOrDefault();
            if (userCheck != null)
            {
                bool check = true;
                return Json(check);
            }
            else
            {
                bool check = false;
                return Json(check);
            }            
        }
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            UserAccount userAccount = db.UserAccounts.Where(ua => ua.Email == email).FirstOrDefault();
            userAccount.Password = "";
            db.SaveChanges();

            string urlComputer = Request.Url.Scheme + "://" + Request.Url.Authority + "/Admin/Login/ResetPassword?userId=";

            string smtpUserName = "c1808j1@gmail.com";
            string smtpPassword = "c1808j1@123";
            string smtpHost = "smtp.gmail.com";
            int smtpPort = 25;
            string emailTo = email;
            string subject = "Reset password";
            string body = string.Format("You received email from: <b>{0}</b><br/>Email: {1}<br/>Nội dung: {2}</br>",
               "Vehicle Showroom Management System", "c1808j1@gmail.com", "Click this link to reset password: "+ urlComputer+userAccount.UserId);
           
           

            EmailService service = new EmailService();
            bool result = service.Send(smtpUserName, smtpPassword, smtpHost, smtpPort, emailTo, subject, body);
            if (result)
            {
                return RedirectToAction("NotificationForgotPassword");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public JsonResult checkEmailForgotPassword(string email)
        {
            bool check = db.UserAccounts.Any(ua => ua.Email == email);
            return Json(check);
        }

        public ActionResult NotificationForgotPassword()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ResetPassword(int userId)
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(string password)
        {
            return RedirectToAction("Index");
        }
    }
}