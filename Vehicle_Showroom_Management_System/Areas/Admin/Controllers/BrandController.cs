using System;
using System.Collections.Generic;
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
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string BrandName, string image)
        {

            db.Insert_Brand(BrandName, image);
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
        public ActionResult Edit(int brandId, string brandName, string image)
        {
            db.Update_Brand(brandId,brandName,image);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public bool Delete(int brandId)
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
            return deleted;
        }
        public ActionResult Details(int brandId)
        {
            Brand detailBrand = db.Brands.Where(ua => ua.BrandId == brandId).FirstOrDefault();
            return View(detailBrand);
        }
    }
}
