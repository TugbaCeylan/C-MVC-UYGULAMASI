using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCOneMagic.Models;

namespace MVCOneMagic.Controllers
{
    public class PostImageController : Controller
    {
        private AppleCatEntities db = new AppleCatEntities();

        // GET: /PostImage/
        public ActionResult Index()
        {
            var postimages = db.PostImages.Include(p => p.Post);
            return View(postimages.ToList());
        }

        // GET: /PostImage/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostImage postimage = db.PostImages.Find(id);
            if (postimage == null)
            {
                return HttpNotFound();
            }
            return View(postimage);
        }

        // GET: /PostImage/Create
        public ActionResult Create()
        {
            ViewBag.PostID = new SelectList(db.Posts, "PostID", "Title");
            return View();
        }

        // POST: /PostImage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ImageID,PostID,ImagePath")] PostImage postimage)
        {
            if (ModelState.IsValid)
            {
                db.PostImages.Add(postimage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostID = new SelectList(db.Posts, "PostID", "Title", postimage.PostID);
            return View(postimage);
        }

        // GET: /PostImage/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostImage postimage = db.PostImages.Find(id);
            if (postimage == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostID = new SelectList(db.Posts, "PostID", "Title", postimage.PostID);
            return View(postimage);
        }

        // POST: /PostImage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ImageID,PostID,ImagePath")] PostImage postimage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(postimage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostID = new SelectList(db.Posts, "PostID", "Title", postimage.PostID);
            return View(postimage);
        }

        // GET: /PostImage/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostImage postimage = db.PostImages.Find(id);
            if (postimage == null)
            {
                return HttpNotFound();
            }
            return View(postimage);
        }

        // POST: /PostImage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PostImage postimage = db.PostImages.Find(id);
            db.PostImages.Remove(postimage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
