using MVCOneMagic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCOneMagic.Controllers
{
    public class CategoryController : Controller
    {
        AppleCatEntities db = new AppleCatEntities();

        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Insert(Category item)
        {
            item.IsDeleted = false;
            if (ModelState.IsValid)
            {
                db.Categories.Add(item);
                bool sonuc = db.SaveChanges() > 0;
                if (sonuc)
                {
                    TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Err);
                }
            }
            else
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Val);
            }
            return View(item);
        }

        public ActionResult Edit(int id)
        {
            return View(db.Categories.Find(id));
        }

        [HttpPost]
        public ActionResult Edit(Category item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(db.Categories.Find(item.CategoryID)).CurrentValues.SetValues(item);
                bool sonuc = db.SaveChanges() > 0;
                if (sonuc)
                {
                    TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Err);
                }
            }
            else
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Val);
            }
            return View();
        }


        public ActionResult Delete(int id)
        {
            Category item = db.Categories.Find(id);

            db.Entry(db.Categories.Find(item.CategoryID)).CurrentValues.SetValues(item.IsDeleted = true);
            bool sonuc = db.SaveChanges() > 0;
            if (sonuc)
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
            }
            else
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Err);
            }

            return RedirectToAction("Index", "Category");

        }
	}
}