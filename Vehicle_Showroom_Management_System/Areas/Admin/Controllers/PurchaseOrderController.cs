using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                        select new {Id = p.Id, PurchaseOrderId = p.PurchaseOrderId, QuantityCarImport = p.QuantityCarImport, ModelCarName = m.ModelCarName,OrderDate = p.OrderDate,Status = p.Status }).ToList();
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
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult Search(int page, int pageSize, DateTime? fromDate, DateTime? toDate)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<PurchaseOrder> listPurchaseOrders = db.PurchaseOrders.ToList();
            List<ModelCar> listModelCars = db.ModelCars.ToList();
            if (fromDate == null)
            {
            var list = (from p in listPurchaseOrders
                        join m in listModelCars on p.ModelCarId equals m.ModelCarId
                            where p.OrderDate >= DateTime.Now && p.OrderDate <= toDate
                        select new { Id = p.Id, PurchaseOrderId = p.PurchaseOrderId, QuantityCarImport = p.QuantityCarImport,ModelCarName = p.ModelCar.ModelCarName,OrderDate = p.OrderDate,Status = p.Status }).ToList();
                var model = list.Skip((page - 1) * pageSize).Take(pageSize);
                var totalRow = list.Count;
                return Json(new
                {
                    data = model,
                    total = totalRow,
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (toDate == null)
                {
                   var list = (from p in listPurchaseOrders
                            join m in listModelCars on p.ModelCarId equals m.ModelCarId
                            where p.OrderDate >= fromDate
                            select new { Id = p.Id, PurchaseOrderId = p.PurchaseOrderId, QuantityCarImport = p.QuantityCarImport, ModelCarName = p.ModelCar.ModelCarName, OrderDate = p.OrderDate, Status = p.Status }).ToList();
                    var model = list.Skip((page - 1) * pageSize).Take(pageSize);
                    var totalRow = list.Count;
                    return Json(new
                    {
                        data = model,
                        total = totalRow,
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                  var list = (from p in listPurchaseOrders
                            join m in listModelCars on p.ModelCarId equals m.ModelCarId
                            where p.OrderDate >= fromDate && p.OrderDate <= toDate
                            select new { Id = p.Id, PurchaseOrderId = p.PurchaseOrderId, QuantityCarImport = p.QuantityCarImport, ModelCarName = p.ModelCar.ModelCarName, OrderDate = p.OrderDate, Status = p.Status }).ToList();
                    var model = list.Skip((page - 1) * pageSize).Take(pageSize);
                    var totalRow = list.Count;
                    return Json(new
                    {
                        data = model,
                        total = totalRow,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
           
         
        }

    }
}