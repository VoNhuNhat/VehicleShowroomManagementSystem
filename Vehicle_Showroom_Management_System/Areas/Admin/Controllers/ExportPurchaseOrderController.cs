using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Vehicle_Showroom_Management_System.Areas.Admin.Data;

namespace Vehicle_Showroom_Management_System.Areas.Admin.Controllers
{
    public class ExportPurchaseOrderController : Controller
    {
        // GET: Admin/ExportPurchaseOrder
        Vehicle_Showroom_Management_SystemEntities db = new Vehicle_Showroom_Management_SystemEntities();

        [HttpGet]
        public ActionResult Index(int Id)
        {
            var list = (from p in db.PurchaseOrders.ToList()
                        join m in db.ModelCars.ToList() on p.ModelCarId equals m.ModelCarId
                        join b in db.Brands.ToList() on m.BrandId equals b.BrandId
                        where p.Id == Id
                        select new
                        {
                            Id = p.Id,
                            PurchaseOrderId = p.PurchaseOrderId,
                            QuantityCarImport = p.QuantityCarImport,
                            ModelCarName = m.ModelCarName,
                            OrderDate = p.OrderDate,
                            CreatedDate = p.CreatedDate,
                            UpdatedDate = p.UpdatedDate,
                            Status = p.Status,
                            ModelCarId = m.ModelCarId,
                            b.BrandName,
                            b.Image,
                            ImportedCar = db.Cars.Where(car => car.Id == p.Id).Count()
                        }).ToList();
            ViewBag.importedCar = list.First().ImportedCar;
            ViewBag.modelCarId = list.First().ModelCarId;
            ViewBag.modelCarName = list.First().ModelCarName;
            ViewBag.brandName = list.First().BrandName;
            ViewBag.imageBrand = list.First().Image;
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Where(p => p.Id == Id).FirstOrDefault();
            return View(purchaseOrder);
        }
        [HttpPost]
        public JsonResult LoadAllCarInPurchaseOrder(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Car> listCars = db.Cars.ToList();
            List<Image> listImages = db.Images.ToList();
            var list = (from c in listCars
                        join i in listImages on c.CarId equals i.CarId
                        orderby c.CreatedDate descending
                        where c.Id == Id && i.Status == 1
                        select new { ModelNumberCar = c.ModelNumberCar,
                            CarName = c.CarName, 
                            PriceInput = c.PriceInput, 
                            PriceOutput = c.PriceOutput, 
                            SeatQuantity = c.SeatQuantity,
                            Color = c.Color, 
                            Gearbox = c.Gearbox,
                            Engine = c.Engine, 
                            Status = c.Status, 
                            Checking = c.Checking,
                            InStock = c.Sold,
                            ImageName = i.Name }).ToList();
            return Json(
                new { 
                data = list,
                } , JsonRequestBehavior.AllowGet);
        }
    }
}