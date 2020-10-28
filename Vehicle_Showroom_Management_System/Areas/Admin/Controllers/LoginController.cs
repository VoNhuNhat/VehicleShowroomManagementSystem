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
                    Session["userId"] = userCheck.UserId;
                    Session["username"] = userCheck.UserName;
                    Session["fullname"] = userCheck.FullName;
                    Session["status"] = userCheck.Status;
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
            char[] charArr = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            Random rd = new Random();
            string randomPassword = String.Empty;
            for (int i = 0; i < 10; i++)
            {
                int pos = rd.Next(1, charArr.Length);
                if (!randomPassword.Contains(charArr.GetValue(pos).ToString()))
                {
                    randomPassword += charArr.GetValue(pos);
                }
                else
                {
                    i--;
                }
            }
            UserAccount userAccount = db.UserAccounts.Where(ua => ua.Email == email).FirstOrDefault();
            userAccount.Password = randomPassword;
            db.SaveChanges();

            string urlComputer = Request.Url.Scheme + "://" + Request.Url.Authority + "/Admin/SetNewPassword/";
            string smtpUserName = "c1808j1@gmail.com";
            string smtpPassword = "c1808j1@123";
            string smtpHost = "smtp.gmail.com";
            int smtpPort = 25;
            string emailTo = email;
            string subject = "Reset password";
            string body = string.Format("We heard that you lost your password. Sorry about that!<br/><br/>But don’t worry! You can use the following link to reset your password:<br/><br/>{0}</br><br/>WARNING: If you don't do this just contact with admin to report this issue !<br/><br/><br/>Thanks,<br/>Vehicle Showroom Management System",
               "Click this link to reset password: "+ urlComputer+userAccount.UserId+"/"+randomPassword);

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
        public ActionResult ResetPassword(int userId,string randomPassword)
        {
            UserAccount userAccount = db.UserAccounts.Where(ua => ua.UserId == userId).FirstOrDefault();
            if (userAccount != null)
            {
                if (randomPassword != null)
                {
                    if (userAccount.Password == randomPassword)
                    {
                    return View();
                    }
                    return RedirectToAction("Index");
                }
                    return RedirectToAction("Index");
            }
                return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult ResetPassword(int userId, string randomPassword, string password)
        {
            UserAccount userAccount = db.UserAccounts.Where(ua => ua.UserId == userId).FirstOrDefault();
            UserAccountController uac = new UserAccountController();
            string ePassword = uac.EncryptPassword(password);
            userAccount.Password = ePassword;
            int check = db.SaveChanges();
            if (check > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("ResetPassword");
            }
        }
    }
}