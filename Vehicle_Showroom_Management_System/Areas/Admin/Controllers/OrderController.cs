using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Showroom_Management_System.Areas.Admin.Data;

namespace Vehicle_Showroom_Management_System.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        Vehicle_Showroom_Management_SystemEntities db = new Vehicle_Showroom_Management_SystemEntities();

        // GET: Admin/Order
        public ActionResult Index()
        {
            List<Order> lists = db.Orders.ToList();
            return View(lists);
        }

        [HttpGet]
        public ActionResult Create(int CustomerId)
        {
            Customer customer = db.Customers.Where(c => c.CustomerId == CustomerId).FirstOrDefault();
            ViewBag.CU = customer;
            return View();
        }

        [HttpPost]
        public JsonResult Create(string ModelNumberCar, int CustomerId, float TotalMoney)
        {
            string id = "1";
            string OrderId = "OC";
            List<Order> listOrders = db.Orders.ToList();
            int count = listOrders.Count();
            Order list = listOrders.OrderByDescending(o => o.CreatedDate).FirstOrDefault();
            if (count > 0)
            {
                id = (list.Id + 1).ToString();
            }
            for (int i = 0; i < 7 - id.Count(); i++)
            {
                OrderId += "0";
            }
            OrderId += id;
            db.Insert_Order(OrderId, ModelNumberCar, CustomerId, TotalMoney);
            db.SaveChanges();
            return Json(db.SaveChanges());
        }
    }
}