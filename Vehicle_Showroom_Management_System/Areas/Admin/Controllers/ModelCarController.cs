using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Showroom_Management_System.Areas.Admin.Data;

namespace Vehicle_Showroom_Management_System.Areas.Admin.Controllers
{
    public class ModelCarController : Controller
    {
        Vehicle_Showroom_Management_SystemEntities db = new Vehicle_Showroom_Management_SystemEntities();
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadData(int page, int pageSize = 2)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<ModelCar> listModelCars = db.ModelCars.ToList();
            List<Brand> listBrands = db.Brands.ToList();
            var list = (from m in listModelCars
                        join b in listBrands on m.BrandId equals b.BrandId
                        orderby m.CreatedDate descending
                        select new { ModelCarId = m.ModelCarId, ModelCarName = m.ModelCarName, BrandName = b.BrandName }).ToList();
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
            List<Brand> list = db.Brands.ToList();
            SelectList listBrand = new SelectList(list, "BrandId", "BrandName");
            ViewBag.CategoryList = listBrand;
            return View();
        }

        [HttpPost]
        public ActionResult Create(string ModelCarName, int BrandId)
        {
            db.Insert_ModeCar(ModelCarName, BrandId);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int ModelCarId)
        {
            ModelCar modelCar = db.ModelCars.Where(m => m.ModelCarId == ModelCarId).FirstOrDefault();
            return View(modelCar);
        }
        [HttpPost]
        public JsonResult CheckModelCarName(string ModelCarName)
        {
            bool existed;
            bool v = db.ModelCars.Any(m => m.ModelCarName == ModelCarName);
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

        [HttpGet]
        public ActionResult Edit(int ModelCarId)
        {
            List<Brand> list = db.Brands.ToList();
            SelectList listBrand = new SelectList(list, "BrandId", "BrandName");
            ViewBag.CategoryList = listBrand;
            ModelCar modelCar = db.ModelCars.Where(m => m.ModelCarId == ModelCarId).FirstOrDefault();
            return View(modelCar);
        }

        [HttpPost]
        public ActionResult Edit(int ModelCarId, string ModelCarName, int BrandId)
        {
            ModelCar modelCar = db.ModelCars.Where(m => m.ModelCarId == ModelCarId).FirstOrDefault();
            modelCar.ModelCarName = ModelCarName;
            modelCar.BrandId = BrandId;
            modelCar.UpdatedDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult CheckModelCarNameEdit(int ModelCarId, string NewModelCarName)
        {
            bool existed;
            bool check = db.ModelCars.Where(ua => ua.ModelCarId != ModelCarId).ToList().Exists(u => u.ModelCarName.Equals(NewModelCarName, StringComparison.CurrentCultureIgnoreCase));
            if (check)
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
        public JsonResult Delete(int ModelCarId)
        {
            bool check = false;
            bool checkExistedPurchaseOrder = db.PurchaseOrders.Any(p => p.ModelCarId == ModelCarId);
            if (!checkExistedPurchaseOrder)
            {
                ModelCar modelCar = db.ModelCars.Where(m => m.ModelCarId == ModelCarId).FirstOrDefault();
                db.ModelCars.Remove(modelCar);
                int d = db.SaveChanges();
                if (d > 0)
                {
                    check = true;
                }
            }
            return Json(check);
        }

        [HttpPost]
        public JsonResult Search(string searchModelCar, int page, int pageSize)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<ModelCar> allModelCars = db.ModelCars.ToList();
            if (searchModelCar != "")
            {
                var list = db.ModelCars.Where(m => m.ModelCarName.Contains(searchModelCar)).Join(
                    db.Brands,
                            m => m.BrandId,
                            b => b.BrandId,
                            (m, b) => new
                            {
                                ModelCarId = m.ModelCarId,
                                ModelCarName = m.ModelCarName,
                                BrandName = b.BrandName
                            }).ToList();
                var model = list.Skip((page - 1) * pageSize).Take(pageSize);
                var totalRow = list.Count;
                if (totalRow > 1)
                {
                    return Json(new
                    {
                        data = model,
                        modelCars = allModelCars,
                        total = totalRow,
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        data = list,
                        modelCars = allModelCars,
                        total = totalRow,
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var model = allModelCars.Skip((page - 1) * pageSize).Take(pageSize);
                var totalRow = allModelCars.Count;
                return Json(new
                {
                    data = model,
                    modelCars = allModelCars,
                    total = totalRow,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}