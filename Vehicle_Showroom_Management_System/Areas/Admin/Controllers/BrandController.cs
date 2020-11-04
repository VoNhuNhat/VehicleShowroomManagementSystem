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
            List<Brand> lists = db.Brands.ToList();
            return View(lists);
        }
        [HttpPost]
        public JsonResult LoadData(int page, int pageSize = 2)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Brand> list = db.Brands.ToList();
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
            return View(brandUpdate);
        }
        [HttpPost]
        public ActionResult Edit(Brand b)
        {
            string fileName = Path.GetFileNameWithoutExtension(b.ImageFile.FileName);
            string extension = Path.GetExtension(b.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(Server.MapPath("~/Areas/Admin/Contents/Images/"), fileName);
            b.Image = fileName;
            b.ImageFile.SaveAs(path);
            Brand b2;
            b2 = db.Brands.Find(b.BrandId);
            string oldPath = Server.MapPath("~/Areas/Admin/Contents/Images/" + b2.Image);
            System.IO.File.Delete(oldPath);
            //if (System.IO.File.Exists(oldPath))
            //{
            //}
            b2.BrandName = b.BrandName;
            b2.Image = b.Image;

            db.Entry(b2).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Delete(int brandId)
        {
            bool deleted;
            db.Brands.Remove(db.Brands.Where(ua => ua.BrandId == brandId).FirstOrDefault());
            int v = db.SaveChanges();
            if (v > 0)
            {
                deleted = true;
            }
            else
            {
                deleted = false;
            }
            return Json(deleted);
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
            List<Brand> allBrands = db.Brands.ToList();
            if (searchBrand != "")
            {
                List<Brand> list = db.Brands.Where(b => b.BrandName.Contains(searchBrand)).ToList();
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
