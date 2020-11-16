using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vehicle_Showroom_Management_System.Areas.Admin.Data;

namespace Vehicle_Showroom_Management_System.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        Vehicle_Showroom_Management_SystemEntities db = new Vehicle_Showroom_Management_SystemEntities();
        // GET: Admin/Brand
        [HttpGet]
        public ActionResult Index()
        {
            if (Convert.ToInt32(Session["status"]) == 1)
            {
            List<Brand> lists = db.Brands.ToList();
            return View(lists);
            }
            else
            {
                return RedirectToAction("Index", "Warning");
            }
        }
        [HttpPost]
        public JsonResult LoadData(int page, int pageSize = 2)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var list = (from b in db.Brands.ToList()
                        orderby b.CreatedDate descending
                        select new
                        {
                            BrandId = b.BrandId,
                            BrandName = b.BrandName,
                            Image = b.Image,
                            CountModelCar = db.ModelCars.Where(m=>m.BrandId == b.BrandId).Count()
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
        public JsonResult CheckBrandName(string brandName)
        {
            bool check = db.Brands.ToList().Exists(u => u.BrandName.Equals(brandName, StringComparison.CurrentCultureIgnoreCase));
            return Json(check);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Brand b)
        {
            string fileName = Path.GetFileNameWithoutExtension(b.ImageFile.FileName);
            string extension = Path.GetExtension(b.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(Server.MapPath("~/Areas/Admin/Contents/Images/"), fileName);
            b.Image = fileName;
            b.ImageFile.SaveAs(path);
            db.Insert_Brand(b.BrandName, b.Image);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int brandId)
        {
            Brand brandUpdate = db.Brands.Where(ua => ua.BrandId == brandId).FirstOrDefault();
            ViewBag.urlPrevious = Request.UrlReferrer.ToString();

            return View(brandUpdate);
        }
        [HttpPost]
        public ActionResult Edit(string urlPrevious, int BrandId, string BrandName, HttpPostedFileBase ImageFile)
        {
            Brand b2;
            Brand updateBrand = db.Brands.Where(b => b.BrandId == BrandId).FirstOrDefault();
            if (ImageFile != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                string extension = Path.GetExtension(ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(Server.MapPath("~/Areas/Admin/Contents/Images/"), fileName);
                ImageFile.SaveAs(path);
                System.IO.File.Delete(Server.MapPath("~/Areas/Admin/Contents/Images/" + updateBrand.Image));
                updateBrand.Image = fileName;
            }
            updateBrand.BrandName = BrandName;
            updateBrand.UpdatedDate = DateTime.Now;

            db.Entry(updateBrand).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect(urlPrevious);
        }

        [HttpPost]
        public JsonResult Delete(int brandId)
        {
            bool deleted;
            if (db.ModelCars.Any(m => m.BrandId == brandId))
            {
                deleted = false;
            }
            else
            {
                Brand brand = db.Brands.Where(ua => ua.BrandId == brandId).FirstOrDefault();
                System.IO.File.Delete(Server.MapPath("~/Areas/Admin/Contents/Images/" + brand.Image));
                db.Brands.Remove(brand);
                int v = db.SaveChanges();
                if (v > 0)
                {
                    deleted = true;
                }
                else
                {
                    deleted = false;
                }
            }
            return Json(deleted);
        }

        [HttpPost]
        public JsonResult CheckExsistBrandName(string oldBrandName, string newBrandName)
        {
            bool check = false;
            if (oldBrandName == "")
            {
                if (db.Brands.Any(b => b.BrandName == newBrandName))
                {
                    check = true;
                }
            }
            else
            {
                if (db.Brands.Where(b => b.BrandName != oldBrandName).Any(b => b.BrandName == newBrandName))
                {
                    check = true;
                }
            }
            return Json(check);
        }
        public ActionResult Details(int brandId, Brand b, string image)
        {

            Brand detailBrand = db.Brands.Where(ua => ua.BrandId == brandId).FirstOrDefault();
            return View(detailBrand);
        }
        [HttpPost]
        public JsonResult Search(string searchBrand, int page, int pageSize)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var allBrands = (from b in db.Brands.ToList()
                                     select new
                                     {
                                         BrandId = b.BrandId,
                                         BrandName = b.BrandName,
                                         Image = b.Image,
                                         CountModelCar = db.ModelCars.Where(m => m.BrandId == b.BrandId).Count()
                                     }).ToList(); ;
            if (searchBrand != "")
            {
                var list = (from b in db.Brands.ToList()
                            where b.BrandName.ToLower().Contains(searchBrand)
                                    select new
                                    {
                                        BrandId = b.BrandId,
                                        BrandName = b.BrandName,
                                        Image = b.Image,
                                        CountModelCar = db.ModelCars.Where(m => m.BrandId == b.BrandId).Count()
                                    }).ToList();
                var model = list.Skip((page - 1) * pageSize).Take(pageSize);
                var totalRow = list.Count;
                if (totalRow > 1)
                {
                    return Json(new
                    {
                        data = model,
                        brands = allBrands,
                        total = totalRow,
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        data = list,
                        brands = allBrands,
                        total = totalRow,
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var model = allBrands.Skip((page - 1) * pageSize).Take(pageSize);
                var totalRow = allBrands.Count;
                return Json(new
                {
                    data = model,
                    brands = allBrands,
                    total = totalRow,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
