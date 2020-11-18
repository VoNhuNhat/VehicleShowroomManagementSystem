using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Showroom_Management_System.Areas.Admin.Data;

namespace Vehicle_Showroom_Management_System.Controllers
{
    public class HomeController : Controller
    {
        Vehicle_Showroom_Management_SystemEntities db = new Vehicle_Showroom_Management_SystemEntities();

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
                            ModelNumberCar = c.ModelNumberCar,
                            CarId = c.CarId,
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
                        }).Take(6).ToList();
            var totalRow = list.Count;
            return Json(new
            {
                data = list,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Team()
        {
            ViewBag.Message = "Your team page.";

            return View();
        }

        public ActionResult Testimonials()
        {
            ViewBag.Message = "Your testimonials page.";

            return View();
        }

        public ActionResult FAQ()
        {
            ViewBag.Message = "Your FAQ page.";

            return View();
        }

        public ActionResult Terms()
        {
            ViewBag.Message = "Your terms page.";

            return View();
        }
    }
}