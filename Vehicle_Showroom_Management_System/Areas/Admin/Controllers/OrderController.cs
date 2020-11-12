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

        [HttpPost]
        public JsonResult LoadData(int page, int pageSize)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Order> listOrders = db.Orders.ToList();
            List<Car> listCars = db.Cars.ToList();
            List<Customer> listCustomers = db.Customers.ToList();
            var list = (from or in listOrders
                        join ca in listCars on or.ModelNumberCar equals ca.ModelNumberCar
                        join cu in listCustomers on or.CustomerId equals cu.CustomerId
                        orderby or.CreatedDate descending
                        select new
                        {
                            Id = or.Id,
                            OrderId = or.OrderId,
                            CarName = ca.CarName,
                            TotalMoney = or.TotalMoney,
                            OrderDate = or.OrderDate,
                            Status = or.Status,
                            FullName = cu.FullName
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
        public JsonResult LoadDataCar(int? Id, int page, int pageSize)
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
                            where i.Status == 1 && c.Checking == 0 && c.Sold == 0
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
                                Sold = c.Sold,
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
                            where i.Status == 1 && c.Id == Id && c.Checking == 0 && c.Sold == 0
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
                                Sold = c.Sold,
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
            db.Update_Car_Sold(ModelNumberCar, 1);
            db.SaveChanges();
            return Json(db.SaveChanges());
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            Order order = db.Orders.Where(o => o.Id == Id).FirstOrDefault();
            return View(order);
        }

        [HttpPost]
        public JsonResult Search(int page, int pageSize, DateTime? fromDate, DateTime? toDate)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var listOrders = db.Orders.ToList();
            var listCars = db.Cars.ToList();
            var listCustomers = db.Customers.ToList();
            if (fromDate == null)
            {
                var list = (from or in listOrders
                            join ca in listCars on or.ModelNumberCar equals ca.ModelNumberCar
                            join cu in listCustomers on or.CustomerId equals cu.CustomerId
                            where or.OrderDate >= DateTime.Now.Date && or.OrderDate <= toDate
                            select new
                            {
                                Id = or.Id,
                                OrderId = or.OrderId,
                                CarName = ca.CarName,
                                TotalMoney = or.TotalMoney,
                                OrderDate = or.OrderDate,
                                Status = or.Status,
                                FullName = cu.FullName
                            }).ToList();
                var model = list.Skip((page - 1) * pageSize).Take(pageSize);
                var totalRow = list.Count;
                return Json(new
                {
                    data = model,
                    total = totalRow,
                    orders = listOrders.Count()
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (toDate == null)
                {
                    var list = (from or in listOrders
                                join ca in listCars on or.ModelNumberCar equals ca.ModelNumberCar
                                join cu in listCustomers on or.CustomerId equals cu.CustomerId
                                where or.OrderDate >= fromDate
                                select new
                                {
                                    Id = or.Id,
                                    OrderId = or.OrderId,
                                    CarName = ca.CarName,
                                    TotalMoney = or.TotalMoney,
                                    OrderDate = or.OrderDate,
                                    Status = or.Status,
                                    FullName = cu.FullName
                                }).ToList();
                    var model = list.Skip((page - 1) * pageSize).Take(pageSize);
                    var totalRow = list.Count;
                    return Json(new
                    {
                        data = model,
                        total = totalRow,
                        orders = listOrders.Count()
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var list = (from or in listOrders
                                join ca in listCars on or.ModelNumberCar equals ca.ModelNumberCar
                                join cu in listCustomers on or.CustomerId equals cu.CustomerId
                                where or.OrderDate >= fromDate && or.OrderDate <= toDate
                                select new
                                {
                                    Id = or.Id,
                                    OrderId = or.OrderId,
                                    CarName = ca.CarName,
                                    TotalMoney = or.TotalMoney,
                                    OrderDate = or.OrderDate,
                                    Status = or.Status,
                                    FullName = cu.FullName
                                }).ToList();
                    var model = list.Skip((page - 1) * pageSize).Take(pageSize);
                    var totalRow = list.Count;
                    return Json(new
                    {
                        data = model,
                        total = totalRow,
                        orders = listOrders.Count()
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public JsonResult SearchCar(string CarNameSearch, string type, int page, int pageSize)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Car> listCars = db.Cars.ToList();
            if (CarNameSearch != "")
            {
                if (type == "Name")
                {
                    var list = (from c in listCars
                                join i in db.Images.ToList() on c.CarId equals i.CarId
                                where i.Status == 1 && c.CarName.ToLower().Contains(CarNameSearch) && c.Checking == 0 && c.Sold == 0
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
                                    Sold = c.Sold,
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
                                where i.Status == 1 && m.ModelCarName.ToLower().Contains(CarNameSearch) && c.Checking == 0 && c.Sold == 0
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
                                    Sold = c.Sold,
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
    }
}