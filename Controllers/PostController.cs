using MVCOneMagic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace MVCOneMagic.Controllers
{
    public class PostController : Controller
    {
        AppleCatEntities db = new AppleCatEntities();

        public ActionResult Index()
        {
            return View(db.Posts.Where(p => p.IsDeleted == false).OrderByDescending(p => p.PostID).ToList());
        }

        public ActionResult Insert()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View(new Post());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Insert(Post item, List<HttpPostedFileBase> fluResim)
        {
            {
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", item.CategoryID);

                try
                {
                    using (TransactionScope ts = new TransactionScope())
                    {

                        item.UserID = Convert.ToInt32(Session["id"]);
                        item.PublishDate = DateTime.Now;
                        item.IsDeleted = false;

                        if (ModelState.IsValid)
                        {
                            db.Posts.Add(item);
                            bool sonuc = db.SaveChanges() > 0;
                            if (sonuc)
                            {
                                foreach (HttpPostedFileBase img in fluResim)
                                {
                                    PostImage pi = new PostImage();
                                    pi.PostID = item.PostID;
                                    bool yuklemeSonucu;
                                    string path = FxFonksiyon.ImageUpload(img, "posts", out yuklemeSonucu);
                                    if (yuklemeSonucu)
                                    {
                                        pi.ImagePath = path;
                                    }
                                    db.PostImages.Add(pi);
                                    db.SaveChanges();
                                }
                                ts.Complete();
                                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
                                return RedirectToAction("Index", "Post");
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
                    }
                }
                catch (Exception)
                {
                    TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Err);
                }
                return View();
            }
        }


        public ActionResult Edit(int id)
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", id);
            return View(db.Posts.Find(id));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Post item, List<HttpPostedFileBase> fluResim)
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", item.CategoryID);
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(db.Posts.Find(item.PostID)).CurrentValues.SetValues(item);
                        bool sonuc = db.SaveChanges() > 0;
                        if (sonuc)
                        {
                            if (fluResim.Count > 0)
                            {
                                foreach (HttpPostedFileBase img in fluResim)
                                {
                                    PostImage pi = new PostImage();
                                    pi.PostID = item.PostID;
                                    bool yuklemeSonucu;
                                    string path = FxFonksiyon.ImageUpload(img, "posts", out yuklemeSonucu);
                                    if (yuklemeSonucu)
                                    {
                                        pi.ImagePath = path;
                                    }
                                    db.PostImages.Add(pi);
                                    db.SaveChanges();
                                }
                            }
                        }

                        TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
                        ts.Complete();
                        return RedirectToAction("Index", "Post");
                    }
                    else
                    {
                        TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Val);
                    }
                }
            }
            catch
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Err);
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            db.Entry(db.Posts.Find(id)).CurrentValues.SetValues(db.Posts.Find(id).IsDeleted = true);
            bool sonuc = db.SaveChanges() > 0;
            if (sonuc)
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
            }
            else
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Err);
            }

            return RedirectToAction("Index", "Post");
        }
	}
}