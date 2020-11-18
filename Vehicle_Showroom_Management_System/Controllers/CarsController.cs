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
            return View();
        }

        [HttpGet]
        public JsonResult LoadData()
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
                            ImageName = i.Name
                        }).ToList();
            var totalRow = list.Count;
            return Json(new
            {
                data = list,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CarDetails(string ModelNumberCar)
        {
            Car carByNumber = db.Cars.Where(c => c.ModelNumberCar == ModelNumberCar).FirstOrDefault();
            List<Image> listImages = db.Images.Where(i => i.CarId == carByNumber.CarId).ToList();
            ViewBag.Images = listImages;
            return View(carByNumber);
        }
    }
}