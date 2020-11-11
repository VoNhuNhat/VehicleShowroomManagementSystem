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
        public JsonResult LoadData(int? Id, int page, int pageSize)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Car> listCars = db.Cars.ToList();
            List<Image> listImages = db.Images.ToList();
            List<PurchaseOrder> listPuchaseOrders = db.PurchaseOrders.ToList();
            List<ModelCar> listModelCars = db.ModelCars.ToList();
            if (Id == null)
            {
               var list = (from c in listCars
                            join i in listImages on c.CarId equals i.CarId
                            join p in listPuchaseOrders on c.Id equals p.Id
                            join m in listModelCars on p.ModelCarId equals m.ModelCarId
                           where i.Status == 1
                           orderby c.CarId descending
                            select new
                            {
                                Id = c.Id,
                                ModelCarName = m.ModelCarName,
                                ModelNumberCar = c.ModelNumberCar,
                                CarName = c.CarName,
                                PriceInput = c.PriceInput,
                                PriceOutput = c.PriceOutput,
                                SeatQuantity = c.SeatQuantity,
                                Color = c.Color,
                                Gearbox = c.Gearbox,
                                Engine = c.Engine,
                                Status = c.Status,
                                Checking = c.Checking,
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
                            join p in listPuchaseOrders on c.Id equals p.Id
                            join m in listModelCars on p.ModelCarId equals m.ModelCarId
                            where i.Status == 1 && c.Id == Id
                            orderby c.CarId descending
                            select new
                            {
                                Id = c.Id,
                                ModelCarName = m.ModelCarName,
                                ModelNumberCar = c.ModelNumberCar,
                                CarName = c.CarName,
                                PriceInput = c.PriceInput,
                                PriceOutput = c.PriceOutput,
                                SeatQuantity = c.SeatQuantity,
                                Color = c.Color,
                                Gearbox = c.Gearbox,
                                Engine = c.Engine,
                                Status = c.Status,
                                Checking = c.Checking,
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
                var firstImage = Images.First();
            foreach (HttpPostedFileBase Image in Images)
            {
                string fileName = Path.GetFileNameWithoutExtension(Image.FileName);
                string extension = Path.GetExtension(Image.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(Server.MapPath("~/Areas/Admin/Contents/Images/"), fileName);
                Image.SaveAs(path);
                Image img = new Image();
                img.CarId = db.Cars.Where(c=>c.ModelNumberCar == ModelNumberCar).FirstOrDefault().CarId;
                img.Name = fileName;
                if (Image.Equals(firstImage))
                {
                img.Status = 1;
                }
                else
                {
                img.Status = 0;
                }
                db.Images.Add(img);
                db.SaveChanges();
                PurchaseOrder purchaseOrder = db.PurchaseOrders.Where(p => p.Id == Id).FirstOrDefault();
                int countCars = db.Cars.Where(c => c.Id == Id).Count();
                if (countCars == purchaseOrder.QuantityCarImport)
                {
                    purchaseOrder.Status = 1;
                    db.SaveChanges();
                }
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

        [HttpPost]
        public JsonResult Search(string CarNameSearch, string type, int page, int pageSize)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Car> listCars = db.Cars.ToList();
            if (CarNameSearch != "")
            {
                if (type == "Name")
                {
                    var list = (from c in listCars
                                join i in db.Images.ToList() on c.CarId equals i.CarId
                                where i.Status == 1 && c.CarName.ToLower().Contains(CarNameSearch)
                                select new
                                {
                                    ModelNumberCar = c.ModelNumberCar,
                                    CarName = c.CarName,
                                    PriceInput = c.PriceInput,
                                    PriceOutput = c.PriceOutput,
                                    SeatQuantity = c.SeatQuantity,
                                    Color = c.Color,
                                    Gearbox = c.Gearbox,
                                    Engine = c.Engine,
                                    Status = c.Status,
                                    Checking = c.Checking,
                                    ImageName = i.Name
                                }).ToList();
                    var model = list.Skip((page - 1) * pageSize).Take(pageSize);
                    var totalRow = list.Count;
                    if (totalRow > 1)
                    {
                        return Json(new
                        {
                            data = model,
                            allCars = listCars.Count(),
                            total = totalRow,
                            status = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            data = list,
                            allCars = listCars.Count(),
                            total = totalRow,
                            status = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var list = (from c in listCars
                                join i in db.Images.ToList() on c.CarId equals i.CarId
                                join p in db.PurchaseOrders.ToList() on c.Id equals p.Id
                                join m in db.ModelCars.ToList() on p.ModelCarId equals m.ModelCarId
                                where i.Status == 1 && m.ModelCarName.ToLower().Contains(CarNameSearch)
                                select new
                                {
                                    ModelNumberCar = c.ModelNumberCar,
                                    CarName = c.CarName,
                                    PriceInput = c.PriceInput,
                                    PriceOutput = c.PriceOutput,
                                    SeatQuantity = c.SeatQuantity,
                                    Color = c.Color,
                                    Gearbox = c.Gearbox,
                                    Engine = c.Engine,
                                    Status = c.Status,
                                    Checking = c.Checking,
                                    ImageName = i.Name
                                }).ToList();
                    var model = list.Skip((page - 1) * pageSize).Take(pageSize);
                    var totalRow = list.Count;
                    if (totalRow > 1)
                    {
                        return Json(new
                        {
                            data = model,
                            allCars = listCars.Count(),
                            total = totalRow,
                            status = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            data = list,
                            allCars = listCars.Count(),
                            total = totalRow,
                            status = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                
            }
            else
            {
                var model = listCars.Skip((page - 1) * pageSize).Take(pageSize);
                var totalRow = listCars.Count;
                return Json(new
                {
                    data = model,
                    allCars = listCars.Count(),
                    total = totalRow,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Edit(string ModelNumberCar)
        {
            Car car = db.Cars.Where(c => c.ModelNumberCar == ModelNumberCar).FirstOrDefault();
            ViewBag.PO = db.PurchaseOrders.Where(p => p.Id == car.Id).FirstOrDefault();
            return View(car);
        }

        [HttpGet]
        public ActionResult Details(string ModelNumberCar)
        {
            return View();
        }


    }
}