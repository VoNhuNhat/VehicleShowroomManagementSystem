using System;
using System.Collections.Generic;
using System.IO;
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
        public ActionResult Create(int Id)
        {
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Where(p => p.Id == Id).FirstOrDefault();
            ViewBag.PO = purchaseOrder;
            return View();
        }

        [HttpPost]
        public ActionResult Create(int Id, string ModelNumberCar, string CarName, float PriceInput, float PriceOutput, int SeatQuantity, string Color, string Gearbox, string Engine, float FuelConsumption, float KilometerGone, int Status, int Checking, DateTime PurchaseOrderDate, HttpPostedFileBase[] Images)
        {
            db.Insert_Car(ModelNumberCar, Id, CarName, PriceInput, PriceOutput, SeatQuantity, Color, Gearbox, Engine, FuelConsumption, KilometerGone, Status, Checking, PurchaseOrderDate);
            db.SaveChanges();
            foreach (HttpPostedFileBase Image in Images)
            {
                string fileName = Path.GetFileNameWithoutExtension(Image.FileName);
                string extension = Path.GetExtension(Image.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(Server.MapPath("~/Areas/Admin/Contents/Images/"), fileName);
                Image.SaveAs(path);
                Image img = new Image();
                img.ModelNumberCar = ModelNumberCar;
                img.Name = Image.FileName;
                db.Images.Add(img);
                db.SaveChanges();
            }
            
            return RedirectToAction("Index","PurchaseOrder");
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