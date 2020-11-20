using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Showroom_Management_System.Areas.Admin.Data;

namespace Vehicle_Showroom_Management_System.Areas.Admin.Controllers
{
    public class ExportOrderController : Controller
    {
        Vehicle_Showroom_Management_SystemEntities db = new Vehicle_Showroom_Management_SystemEntities();
        [HttpGet]
        // GET: Admin/ExportOrder
        public ActionResult Index(int Id)
        {
            Order order = db.Orders.Where(o => o.Id == Id).FirstOrDefault();
            Car carGetByModelNumberCar = db.Cars.Where(c => c.ModelNumberCar == order.ModelNumberCar).FirstOrDefault();
            var list = (from p in db.PurchaseOrders.ToList()
                        join m in db.ModelCars.ToList() on p.ModelCarId equals m.ModelCarId
                        join b in db.Brands.ToList() on m.BrandId equals b.BrandId
                        where p.Id == carGetByModelNumberCar.Id
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
                        }).ToList();

            ViewBag.modelCarId = list.First().ModelCarId;
            ViewBag.modelCarName = list.First().ModelCarName;
            ViewBag.brandName = list.First().BrandName;
            ViewBag.imageBrand = list.First().Image;
            ViewBag.seat = carGetByModelNumberCar.SeatQuantity;
            ViewBag.gearBox = carGetByModelNumberCar.Gearbox;
            ViewBag.engine = carGetByModelNumberCar.Engine;
            ViewBag.fuel = carGetByModelNumberCar.FuelConsumption;
            ViewBag.km = carGetByModelNumberCar.KilometerGone;

            var customer = (from o in db.Orders.ToList()
                            join c in db.Customers.ToList() on o.CustomerId equals c.CustomerId
                            join u in db.UserAccounts.ToList() on c.UserId equals u.UserId
                            where o.Id == Id
                            select new
                            {
                                c.FullName,
                                c.Address,
                                c.Email,
                                c.Phone,
                                FullNameUser = u.FullName
                            }).ToList();

            ViewBag.seller = customer.First().FullNameUser;
            ViewBag.fullname = customer.First().FullName;
            ViewBag.address = customer.First().Address;
            ViewBag.email = customer.First().Email;
            ViewBag.phone = customer.First().Phone;

            return View(order);
        }

        [HttpPost]
        public JsonResult LoadCarInOrder(string ModelNumberCar)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var list = (from c in db.Cars.ToList()
             join i in db.Images.ToList() on c.CarId equals i.CarId
             where c.ModelNumberCar == ModelNumberCar && i.Status == 1
             select new
             {
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
                 ImageName = i.Name
             }).ToList();
      
            return Json(
                new
                {
                    data = list,
                }, JsonRequestBehavior.AllowGet);
        }
    }
}