using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Showroom_Management_System.Areas.Admin.Data;

namespace Vehicle_Showroom_Management_System.Controllers
{
    public class CarsController : Controller
    {
        Vehicle_Showroom_Management_SystemEntities db = new Vehicle_Showroom_Management_SystemEntities();

        // GET: Cars
        public ActionResult Index()
        {
            List<Brand> listBrands = db.Brands.ToList();
            List<ModelCar> listModelCars = db.ModelCars.ToList();
            ViewBag.Brands = listBrands;
            ViewBag.ModelCars = listModelCars;
            return View();
        }

        [HttpGet]
        public JsonResult LoadData(int page, int pageSize)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Car> listCars = db.Cars.ToList();
            List<Image> listImages = db.Images.ToList();
            var list = (from c in listCars
                        join i in listImages on c.CarId equals i.CarId
                        where i.Status == 1 && c.Checking == 0 && c.Sold == 0
                        orderby c.CreatedDate descending
                        select new
                        {
                            CarId = c.CarId,
                            ModelNumberCar = c.ModelNumberCar,
                            KilometerGone = c.KilometerGone,
                            Engine = c.Engine,
                            Gearbox = c.Gearbox,
                            CarName = c.CarName,
                            Color = c.Color,
                            FuelConsumption = c.FuelConsumption,
                            PurchaseOrderDate = c.PurchaseOrderDate,
                            Status = c.Status,
                            PriceOutput = c.PriceOutput,
                            CreatedDate = c.CreatedDate,
                            ImageName = i.Name
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

        [HttpPost]
        public JsonResult Search(int page, int pageSize, int? brandId, int? modelCarId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Car> listCars = db.Cars.ToList();
            List<Image> listImages = db.Images.ToList();
            List<PurchaseOrder> listPurchaseOrders = db.PurchaseOrders.ToList();
            List<ModelCar> listModelCars = db.ModelCars.ToList();
            List<Brand> listBrands = db.Brands.ToList();
            if (brandId == null)
            {
                if (modelCarId == null)
                {
                    var list = (from c in listCars
                                join i in listImages on c.CarId equals i.CarId
                                where i.Status == 1 && c.Checking == 0 && c.Sold == 0
                                orderby c.CreatedDate descending
                                select new
                                {
                                    CarId = c.CarId,
                                    ModelNumberCar = c.ModelNumberCar,
                                    KilometerGone = c.KilometerGone,
                                    Engine = c.Engine,
                                    Gearbox = c.Gearbox,
                                    CarName = c.CarName,
                                    Color = c.Color,
                                    FuelConsumption = c.FuelConsumption,
                                    PurchaseOrderDate = c.PurchaseOrderDate,
                                    Status = c.Status,
                                    PriceOutput = c.PriceOutput,
                                    CreatedDate = c.CreatedDate,
                                    ImageName = i.Name
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
                else
                {
                    var list = (from c in listCars
                                join i in listImages on c.CarId equals i.CarId
                                join po in listPurchaseOrders on c.Id equals po.Id
                                join mc in listModelCars on po.ModelCarId equals mc.ModelCarId
                                where i.Status == 1 && c.Checking == 0 && c.Sold == 0 && mc.ModelCarId == modelCarId
                                orderby c.CreatedDate descending
                                select new
                                {
                                    CarId = c.CarId,
                                    ModelNumberCar = c.ModelNumberCar,
                                    KilometerGone = c.KilometerGone,
                                    Engine = c.Engine,
                                    Gearbox = c.Gearbox,
                                    CarName = c.CarName,
                                    Color = c.Color,
                                    FuelConsumption = c.FuelConsumption,
                                    PurchaseOrderDate = c.PurchaseOrderDate,
                                    Status = c.Status,
                                    PriceOutput = c.PriceOutput,
                                    CreatedDate = c.CreatedDate,
                                    ImageName = i.Name
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
            }
            else
            {
                if (modelCarId == null)
                {
                    var list = (from c in listCars
                                join i in listImages on c.CarId equals i.CarId
                                join po in listPurchaseOrders on c.Id equals po.Id
                                join mc in listModelCars on po.ModelCarId equals mc.ModelCarId
                                join b in listBrands on mc.BrandId equals b.BrandId
                                where i.Status == 1 && c.Checking == 0 && c.Sold == 0 && b.BrandId == brandId
                                orderby c.CreatedDate descending
                                select new
                                {
                                    CarId = c.CarId,
                                    ModelNumberCar = c.ModelNumberCar,
                                    KilometerGone = c.KilometerGone,
                                    Engine = c.Engine,
                                    Gearbox = c.Gearbox,
                                    CarName = c.CarName,
                                    Color = c.Color,
                                    FuelConsumption = c.FuelConsumption,
                                    PurchaseOrderDate = c.PurchaseOrderDate,
                                    Status = c.Status,
                                    PriceOutput = c.PriceOutput,
                                    CreatedDate = c.CreatedDate,
                                    ImageName = i.Name
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
                else
                {
                    var list = (from c in listCars
                                join i in listImages on c.CarId equals i.CarId
                                join po in listPurchaseOrders on c.Id equals po.Id
                                join mc in listModelCars on po.ModelCarId equals mc.ModelCarId
                                join b in listBrands on mc.BrandId equals b.BrandId
                                where i.Status == 1 && c.Checking == 0 && c.Sold == 0 && b.BrandId == brandId && mc.ModelCarId == modelCarId
                                orderby c.CreatedDate descending
                                select new
                                {
                                    CarId = c.CarId,
                                    ModelNumberCar = c.ModelNumberCar,
                                    KilometerGone = c.KilometerGone,
                                    Engine = c.Engine,
                                    Gearbox = c.Gearbox,
                                    CarName = c.CarName,
                                    Color = c.Color,
                                    FuelConsumption = c.FuelConsumption,
                                    PurchaseOrderDate = c.PurchaseOrderDate,
                                    Status = c.Status,
                                    PriceOutput = c.PriceOutput,
                                    CreatedDate = c.CreatedDate,
                                    ImageName = i.Name
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
            }
        }

        [HttpGet]
        public JsonResult Combobox(int? brandId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<ModelCar> listModelCars = db.ModelCars.ToList();
            if (brandId == null)
            {
                return Json(new
                {
                    data = listModelCars,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var modelCars = listModelCars.Where(mc => mc.BrandId == brandId).ToList();
                return Json(new
                {
                    data = modelCars,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult CarDetails(string ModelNumberCar)
        {
            Car carByNumber = db.Cars.Where(c => c.ModelNumberCar == ModelNumberCar).FirstOrDefault();
            PurchaseOrder purchaseOrderById = db.PurchaseOrders.Where(po => po.Id == carByNumber.Id).FirstOrDefault();
            ModelCar modelCarById = db.ModelCars.Where(mc => mc.ModelCarId == purchaseOrderById.ModelCarId).FirstOrDefault();
            Brand brandById = db.Brands.Where(b => b.BrandId == modelCarById.BrandId).FirstOrDefault();
            List<Image> listImages = db.Images.Where(i => i.CarId == carByNumber.CarId).ToList();
            DateTime date = (DateTime)carByNumber.CreatedDate;
            ViewBag.CreatedDate = date.ToString("MM-dd-yyyy");
            ViewBag.Images = listImages;
            ViewBag.ModelCars = modelCarById.ModelCarName;
            ViewBag.Brands = brandById.BrandName;
            return View(carByNumber);
        }
    }
}