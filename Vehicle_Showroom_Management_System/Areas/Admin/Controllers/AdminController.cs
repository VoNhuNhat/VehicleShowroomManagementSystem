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
                ViewBag.fullname = Session["fullname"];
                return View();
            }
            else
            {
                return RedirectToAction("Index","Login");
            }
        }

    }
}