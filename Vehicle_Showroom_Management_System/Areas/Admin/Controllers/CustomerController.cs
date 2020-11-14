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
        public JsonResult LoadData(int page, int pageSize = 2)
        {
            if (Convert.ToInt32(Session["status"]) == 1)
            {
                db.Configuration.ProxyCreationEnabled = false;
                List<UserAccount> listUsers = db.UserAccounts.ToList();
                List<Customer> listCustomers = db.Customers.ToList();
                var list = (from c in listCustomers
                            join u in listUsers on c.UserId equals u.UserId
                            orderby c.CreatedDate descending
                            select new { CustomerId = c.CustomerId, FullName = c.FullName, Email = c.Email, Phone = c.Phone, userFullName = u.FullName }).ToList();
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
            else
            {
                int userId = Convert.ToInt32(Session["userId"]);
                db.Configuration.ProxyCreationEnabled = false;
                List<UserAccount> listUsers = db.UserAccounts.ToList();
                List<Customer> listCustomers = db.Customers.ToList();
                var list = (from c in listCustomers
                            where c.UserId == userId
                            join u in listUsers on c.UserId equals u.UserId
                            orderby c.CreatedDate descending
                            select new { CustomerId = c.CustomerId, FullName = c.FullName, Email = c.Email, Phone = c.Phone, userFullName = u.FullName }).ToList();
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
            bool deleted = false;
            bool checkExistedOrder = db.Orders.Any(o => o.CustomerId == customerId);
            if (!checkExistedOrder)
            {
                Customer customerDelete = db.Customers.Where(c => c.CustomerId == customerId).FirstOrDefault();
                db.Customers.Remove(customerDelete);
                int d = db.SaveChanges();
                if (d > 0)
                {
                    deleted = true;
                }
            }
            return Json(deleted);
        }

        [HttpPost]
        public JsonResult Search(string searchCustomer, int page, int pageSize)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Customer> allCustomers = db.Customers.ToList();
            if (searchCustomer != "")
            {
                if (Convert.ToInt32(Session["status"]) == 1)
                {
                    var list = db.Customers.Where(c => c.FullName.Contains(searchCustomer)).Join(
                            db.UserAccounts,
                            c => c.UserId,
                            u => u.UserId,
                            (c, u) => new
                            {
                                CustomerId = c.CustomerId,
                                FullName = c.FullName,
                                Email = c.Email,
                                Phone = c.Phone,
                                userFullName = u.FullName
                            }).ToList();
                    var model = list.Skip((page - 1) * pageSize).Take(pageSize);
                    var totalRow = list.Count;
                    if (totalRow > 1)
                    {
                        return Json(new
                        {
                            data = model,
                            customers = allCustomers,
                            total = totalRow,
                            status = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            data = list,
                            customers = allCustomers,
                            total = totalRow,
                            status = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    int userId = Convert.ToInt32(Session["userId"]);
                    var list = db.Customers.Where(c => c.FullName.Contains(searchCustomer) && c.UserId == userId).Join(
                            db.UserAccounts,
                            c => c.UserId,
                            u => u.UserId,
                            (c, u) => new
                            {
                                CustomerId = c.CustomerId,
                                FullName = c.FullName,
                                Email = c.Email,
                                Phone = c.Phone,
                                userFullName = u.FullName
                            }).ToList();
                    var model = list.Skip((page - 1) * pageSize).Take(pageSize);
                    var totalRow = list.Count;
                    if (totalRow > 1)
                    {
                        return Json(new
                        {
                            data = model,
                            customers = allCustomers,
                            total = totalRow,
                            status = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            data = list,
                            customers = allCustomers,
                            total = totalRow,
                            status = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                var model = allCustomers.Skip((page - 1) * pageSize).Take(pageSize);
                var totalRow = allCustomers.Count;
                return Json(new
                {
                    data = model,
                    customers = allCustomers,
                    total = totalRow,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}