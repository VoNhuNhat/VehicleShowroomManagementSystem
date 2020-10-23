﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Showroom_Management_System.Areas.Admin.Data;
using Vehicle_Showroom_Management_System.Areas.Admin.Controllers;

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
            UserAccount userCheck = db.UserAccounts.Where(ua => ua.UserName == username && ua.Password == password).FirstOrDefault();
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
    }
}