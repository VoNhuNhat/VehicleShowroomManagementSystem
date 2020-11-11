using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Vehicle_Showroom_Management_System.Areas.Admin.Data;

namespace Vehicle_Showroom_Management_System.Areas.Admin.Controllers
{
    public class PurchaseOrderController : Controller
    {
        // GET: Admin/PurchaseOrder
        Vehicle_Showroom_Management_SystemEntities db = new Vehicle_Showroom_Management_SystemEntities();

        public ActionResult Index()
        {
            List<PurchaseOrder> list = db.PurchaseOrders.ToList();
            return View(list);
        }

        [HttpPost]
        public JsonResult LoadData(int page, int pageSize)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<PurchaseOrder> listPurchaseOrders = db.PurchaseOrders.ToList();
            List<ModelCar> listModelCars = db.ModelCars.ToList();
            var list = (from p in listPurchaseOrders
                        join m in listModelCars on p.ModelCarId equals m.ModelCarId
                        orderby p.CreatedDate descending
                        select new {
                            Id = p.Id, 
                            PurchaseOrderId = p.PurchaseOrderId, 
                            QuantityCarImport = p.QuantityCarImport,
                            ModelCarName = m.ModelCarName,
                            OrderDate = p.OrderDate,
                            Status = p.Status,
                            ImportedCar = db.Cars.Where(car=>car.Id == p.Id).Count()
                        }).ToList();
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
            List<ModelCar> list = db.ModelCars.ToList();
            SelectList listModelCar = new SelectList(list, "ModelCarId", "ModelCarName");
            ViewBag.ModelCarList = listModelCar;
            return View();
        }
        [HttpPost]
        public ActionResult Create(int ModelCarId,int QuantityCarImport,DateTime OrderDate)
        {
            string id = "1";
            string PurchaseaOrderId = "SC";
            List<PurchaseOrder> listPurchaseOrders = db.PurchaseOrders.ToList();
            int count = listPurchaseOrders.Count();
            PurchaseOrder list = listPurchaseOrders.OrderByDescending(p => p.CreatedDate).FirstOrDefault();
            if (count > 0)
            {
                id = (list.Id + 1).ToString();
            }
            for (int i = 0; i < 7 - id.Count(); i++)
            {
                PurchaseaOrderId += "0";
            }
            PurchaseaOrderId += id;
            db.Insert_PurchaseOrder(PurchaseaOrderId, ModelCarId, QuantityCarImport, OrderDate);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            List<ModelCar> list = db.ModelCars.ToList();
            SelectList listModelCar = new SelectList(list, "ModelCarId", "ModelCarName");
            ViewBag.ModelCarList = listModelCar;
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Where(p => p.Id == Id).FirstOrDefault();
            return View(purchaseOrder);
        }
        [HttpPost]
        public ActionResult Edit(int Id, int ModelCarId, int QuantityCarImport, DateTime OrderDate)
        {
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Where(p => p.Id == Id).FirstOrDefault();
            purchaseOrder.ModelCarId = ModelCarId;
            purchaseOrder.QuantityCarImport = QuantityCarImport;
            purchaseOrder.OrderDate = OrderDate;
            purchaseOrder.UpdatedDate = DateTime.Now;
            db.SaveChanges();
            int countCars = db.Cars.Where(c => c.Id == Id).Count();
            if (QuantityCarImport > countCars)
            {
                purchaseOrder.Status = 0;
                db.SaveChanges();
            }
            else
            {
                purchaseOrder.Status = 1;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult Search(int page, int pageSize, DateTime? fromDate, DateTime? toDate)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var listPurchaseOrders = db.PurchaseOrders.ToList();
            var listModelCars = db.ModelCars.ToList();
            if (fromDate == null)
            {
            var list = (from p in listPurchaseOrders
                        join m in listModelCars on p.ModelCarId equals m.ModelCarId
                            where p.OrderDate >= DateTime.Now.Date && p.OrderDate <= toDate
                        select new { 
                            Id = p.Id, 
                            PurchaseOrderId = p.PurchaseOrderId,
                            QuantityCarImport = p.QuantityCarImport,
                            ModelCarName = p.ModelCar.ModelCarName,
                            OrderDate = p.OrderDate,
                            Status = p.Status, 
                            ImportedCar = db.Cars.Where(car => car.Id == p.Id).Count() 
                        }).ToList();
                var model = list.Skip((page - 1) * pageSize).Take(pageSize);
                var totalRow = list.Count;
                return Json(new
                {
                    data = model,
                    total = totalRow,
                    purchaseOrders = listPurchaseOrders.Count()
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (toDate == null)
                {
                    var list = (from p in listPurchaseOrders
                            join m in listModelCars on p.ModelCarId equals m.ModelCarId
                            where p.OrderDate >= fromDate
                            select new { Id = p.Id, PurchaseOrderId = p.PurchaseOrderId, QuantityCarImport = p.QuantityCarImport, ModelCarName = p.ModelCar.ModelCarName, OrderDate = p.OrderDate, Status = p.Status, ImportedCar = db.Cars.Where(car => car.Id == p.Id).Count() }).ToList();
                    var model = list.Skip((page - 1) * pageSize).Take(pageSize);
                    var totalRow = list.Count;
                    return Json(new
                    {
                        data = model,
                        total = totalRow,
                        purchaseOrders = listPurchaseOrders.Count()
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var list = (from p in listPurchaseOrders
                            join m in listModelCars on p.ModelCarId equals m.ModelCarId
                            where p.OrderDate >= fromDate && p.OrderDate <= toDate
                            select new { Id = p.Id, PurchaseOrderId = p.PurchaseOrderId, QuantityCarImport = p.QuantityCarImport, ModelCarName = p.ModelCar.ModelCarName, OrderDate = p.OrderDate, Status = p.Status, ImportedCar = db.Cars.Where(car => car.Id == p.Id).Count() }).ToList();
                    var model = list.Skip((page - 1) * pageSize).Take(pageSize);
                    var totalRow = list.Count;
                    return Json(new
                    {
                        data = model,
                        total = totalRow,
                        purchaseOrders = listPurchaseOrders.Count()
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpGet]
        public ActionResult Details(int Id)
        {
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Where(p => p.Id == Id).FirstOrDefault();
            ViewBag.importedCar = db.Cars.Where(c => c.Id == Id).Count();
            return View(purchaseOrder);
        }
        [HttpPost]
        public JsonResult LoadCars(int page,int pageSize,int Id)
        {
            List<Car> listCars = db.Cars.ToList();
            List<Image> listImages = db.Images.ToList();
            var list = (from c in listCars
                        join i in listImages on c.CarId equals i.CarId
                        where c.Id == Id
                        select new { ModelNumberCar= c.ModelNumberCar, CarName=c.CarName, PriceInput=c.PriceInput, PriceOutput=c.PriceOutput, SeatQuantity=c.SeatQuantity, Color=c.Color, Gearbox=c.Gearbox, Engine=c.Engine, Status=c.Status, Checking=c.Checking,ImageName = i.Name }).ToList();
            db.Configuration.ProxyCreationEnabled = false;
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

        [HttpPost]
        public JsonResult CheckImportCar(int Id)
        {
            bool ok = true;
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Where(p => p.Id == Id).FirstOrDefault();
            if (purchaseOrder.Status == 1)
            {
                ok = false;
            }
            return Json(ok);
        }

        [HttpPost]
        public JsonResult CheckQuantityEditCar(int Id,int quantityImportedCar)
        {
            bool ok = true;
            int countCars = db.Cars.Where(c => c.Id == Id).Count();
            if (quantityImportedCar < countCars)
            {
                ok = false;
            }
            return Json(ok);
        }
        [HttpPost]
        public JsonResult Delete(int Id)
        {
            bool deleted = false;
            bool checkExistedCar = db.Cars.Any(c => c.Id == Id);
            if (!checkExistedCar)
            {
                PurchaseOrder purchaseOrderDelete = db.PurchaseOrders.Where(p => p.Id == Id).FirstOrDefault();
                db.PurchaseOrders.Remove(purchaseOrderDelete);
                int d = db.SaveChanges();
                if (d > 0)
                {
                    deleted = true;
                }
            }
            return Json(deleted);
        }

    }
}