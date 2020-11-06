using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Showroom_Management_System.Areas.Admin.Data;

namespace Vehicle_Showroom_Management_System.Areas.Admin.Controllers
{
    public class CarController : Controller
    {
        Vehicle_Showroom_Management_SystemEntities db = new Vehicle_Showroom_Management_SystemEntities();

        // GET: Admin/Car
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadData(int page, int pageSize = 2)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Car> list = db.Cars.ToList();
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
            List<PurchaseOrder> purchaseOrders = db.PurchaseOrders.Where(po=>po.Status == 0).ToList();
            List<ModelCar> modelCars = db.ModelCars.ToList();
            var list = from po in purchaseOrders
                       join mc in modelCars on po.ModelCarId equals mc.ModelCarId
                       select new { PurchaseOrderId = po.PurchaseOrderId, ModelCarName = mc.ModelCarName };
            SelectList listPurchaseOrder = new SelectList(list, "PurchaseOrderId", "ModelCarName");
            ViewBag.PurchaseOrderList = listPurchaseOrder;
            return View();
        }

        [HttpPost]
        public ActionResult Create(string modelNumberCar, int purchaseOrderId, string carName, float priceInput, float priceOutput, int seatQuantity, string color, string gearbox, string engine, float fuelConsumption, float kilometerGone, int status, int checking, DateTime purchaseOrderDate)
        {
            db.Insert_Car(modelNumberCar, purchaseOrderId, carName, priceInput, priceOutput, seatQuantity, color, gearbox, engine, fuelConsumption, kilometerGone, status, checking, purchaseOrderDate);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult CheckModelNumberCar(string modelNumberCar)
        {
            bool existed;
            bool v = db.Cars.Any(c => c.ModelNumberCar == modelNumberCar);
            if (v)
            {
                existed = true;
            }
            else
            {
                existed = false;
            }
            return Json(existed);
        }
    }
}