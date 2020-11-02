using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Showroom_Management_System.Areas.Admin.Data;

namespace Vehicle_Showroom_Management_System.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        Vehicle_Showroom_Management_SystemEntities db = new Vehicle_Showroom_Management_SystemEntities();

        // GET: Admin/Customer
        [HttpGet]
        public ActionResult Index()
        {
            List<Customer> list = db.Customers.ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string fullName, string address, string email, string phone, DateTime birthday)
        {
            int userId = Convert.ToInt32(Session["userId"]);
            db.Insert_Customer(userId, fullName, address, email, phone, birthday);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult CheckEmail(string email)
        {
            bool check = db.Customers.ToList().Exists(c => c.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
            return Json(check);
        }

        [HttpPost]
        public JsonResult CheckPhone(string phone)
        {
            bool check = db.Customers.ToList().Exists(c => c.Phone.Equals(phone, StringComparison.CurrentCultureIgnoreCase));
            return Json(check);
        }

        [HttpGet]
        public ActionResult Details(int customerId)
        {
            Customer detailCustomer = db.Customers.Where(c => c.CustomerId == customerId).FirstOrDefault();
            return View(detailCustomer);
        }

        [HttpGet]
        public ActionResult Edit(int customerId)
        {
            Customer customerUpdate = db.Customers.Where(c => c.CustomerId == customerId).FirstOrDefault();
            return View(customerUpdate);
        }

        [HttpPost]
        public ActionResult Edit(int customerId, string fullName, string address, string email, string phone, DateTime birthday)
        {
            db.Update_Customer(customerId, fullName, address, email, phone, birthday);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult CheckEmailEdit(string oldEmail, string newEmail)
        {
            bool check = db.Customers.Where(c => c.Email != oldEmail).ToList().Exists(c => c.Email.Equals(newEmail, StringComparison.CurrentCultureIgnoreCase));
            return Json(check);
        }

        [HttpPost]
        public JsonResult CheckPhoneEdit(string oldPhone, string newPhone)
        {
            bool check = db.Customers.Where(c => c.Phone != oldPhone).ToList().Exists(c => c.Phone.Equals(newPhone, StringComparison.CurrentCultureIgnoreCase));
            return Json(check);
        }

        [HttpPost]
        public JsonResult Delete(int customerId)
        {
            bool deleted;
            db.Customers.Remove(db.Customers.Where(c => c.CustomerId == customerId).FirstOrDefault());
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
    }
}