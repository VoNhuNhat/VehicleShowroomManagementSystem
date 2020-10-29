using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Showroom_Management_System.Areas.Admin.Data;
using PagedList;

namespace Vehicle_Showroom_Management_System.Areas.Admin.Controllers
{
    public class UserAccountController : Controller
    {
        Vehicle_Showroom_Management_SystemEntities db = new Vehicle_Showroom_Management_SystemEntities();
       [HttpGet]
        public ActionResult Index()
        {
            if (Convert.ToInt32(Session["status"]) == 1)
            {
            List<UserAccount> list = db.UserAccounts.Where(ua=>ua.Status != 1).ToList();
            return View(list);
            }
            else
            {
                return View("WarningUser");
            }
        }
        [HttpGet]
        public JsonResult LoadData(int page, int pageSize = 2)
        {
            List<UserAccount> list = db.UserAccounts.Where(ua => ua.Status != 1).OrderByDescending(ua => ua.CreatedDate).ToList();
            var model = list.Skip((page - 1) * pageSize).Take(pageSize);
            var totalRow = list.Count;
            if (totalRow > 1)
            {
                return Json(new
                {
                    data = model,
                    total = totalRow,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
            return Json(new
            {
               data = list,
               total = totalRow,
               status = true
            }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string fullName, string userName, string password, string address, string email, string phoneNumber,DateTime birthday)
        {
            string ePassword = EncryptPassword(password);
            db.Insert_UserAccount(fullName, userName, ePassword, address, email, phoneNumber,birthday);
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

        [HttpGet]
        public ActionResult Edit(int userId)
        {
            UserAccount userUpdate = db.UserAccounts.Where(ua => ua.UserId == userId).FirstOrDefault();
            userUpdate.Password = DecryptPassword(userUpdate.Password);
            return View(userUpdate);
        }
        [HttpPost]
        public ActionResult Edit(int userId, string fullName, string userName, string password, string address, string email, string phoneNumber, DateTime birthday)
        {
            string ePassword = EncryptPassword(password);
            db.Update_UserAccount(userId, fullName, userName, ePassword, address, email, phoneNumber, birthday);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult CheckUserEdit(string oldUserName, string newUserName)
        {
            bool check = db.UserAccounts.Where(ua => ua.UserName != oldUserName).ToList().Exists(u => u.UserName.Equals(newUserName, StringComparison.CurrentCultureIgnoreCase));
            return Json(check);
        }
        [HttpPost]
        public JsonResult CheckEmailEdit(string oldEmail, string newEmail)
        {
            bool check = db.UserAccounts.Where(ua => ua.Email != oldEmail).ToList().Exists(u => u.Email.Equals(newEmail, StringComparison.CurrentCultureIgnoreCase));
            return Json(check);
        }
        [HttpPost]
        public JsonResult CheckPhoneNumberEdit(string oldPhoneNumber, string newPhoneNumber)
        {
            bool check = db.UserAccounts.Where(ua => ua.PhoneNumber != oldPhoneNumber).ToList().Exists(u => u.PhoneNumber.Equals(newPhoneNumber, StringComparison.CurrentCultureIgnoreCase));
            return Json(check);
        }

        public string EncryptPassword(string password)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(password);
            string encryptedPassword = Convert.ToBase64String(b);
            return encryptedPassword;
        }

        public string DecryptPassword(string encryptedPassword)
        {
            byte[] b;
            string decryptedPassword;

            try
            {
                b = Convert.FromBase64String(encryptedPassword);
                decryptedPassword = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException)
            {
                decryptedPassword = "";
            }

            return decryptedPassword;
        }
        [HttpPost]
        public JsonResult Delete(int userId)
        {
            bool deleted;
            db.UserAccounts.Remove(db.UserAccounts.Where(ua => ua.UserId == userId).FirstOrDefault());
            int v = db.SaveChanges();
            if (v > 0)
            {
                deleted = true;
            }
            else
            {
                deleted = false;
            }
            return Json(deleted);
        }
        [HttpGet]
        public ActionResult Details(int userId)
        {
            UserAccount detailUser = db.UserAccounts.Where(ua => ua.UserId == userId).FirstOrDefault();
            return View(detailUser);
        }
        [HttpPost]
        public JsonResult Search(string searchUser, int page, int pageSize)
        {
            List<UserAccount> allUsers = db.UserAccounts.Where(ua => ua.Status == 0).ToList();
            if (searchUser != "")
            {
            List<UserAccount> list = db.UserAccounts.Where(ua => ua.FullName.Contains(searchUser) && ua.Status == 0).ToList();
                var model = list.Skip((page - 1) * pageSize).Take(pageSize);
                var totalRow = list.Count;
                if (totalRow > 1)
                {
                    return Json(new
                    {
                    data = model,
                    users = allUsers,
                    total = totalRow,
                    status = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        data = list,
                        users = allUsers,
                        total = totalRow,
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var model = allUsers.Skip((page - 1) * pageSize).Take(pageSize);
                var totalRow = allUsers.Count;
                return Json(new
                {
                    data = model,
                    users = allUsers,
                    total = totalRow,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}