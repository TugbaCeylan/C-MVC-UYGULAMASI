using MVCOneMagic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCOneMagic.Controllers
{
    public class HomeController : Controller
    {
        AppleCatEntities db = new AppleCatEntities();
       

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User item)
        {

            if (ModelState.IsValid)
            {
                item.RoleID = 2;
                item.IsDeleted = false;
                item.Date = DateTime.Now;

                db.Users.Add(item);
                bool sonuc = db.SaveChanges() > 0;
                if (sonuc)
                {
                    TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
                    return View();
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

        public ActionResult Login(User _model)
        {
            if (db.Users.Any(u => u.FirstName == _model.FirstName && u.Password == _model.Password))
            {
                User girisYapan = db.Users.Where(u => u.FirstName == _model.FirstName && u.Password == _model.Password).FirstOrDefault();
                Session["oturum"] = girisYapan;
                Session["ad"] = girisYapan.FirstName;
                Session["soyad"] = girisYapan.LastName;
                Session["id"] = girisYapan.UserID;
                Session["rolId"] = girisYapan.RoleID;


                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        public ActionResult Exit()
        {
            Session.Abandon();
            return RedirectToAction("Create", "Home");
        }

        public ActionResult Forget()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Forget(User _model)
        {
           

            if (db.Users.Any(u => u.FirstName == _model.FirstName && u.Email == _model.Email && u.PasswordQuestion == _model.PasswordQuestion && u.PaswordAnswer == _model.PaswordAnswer))
            {

                User sifreUnutan = db.Users.Where(u => u.FirstName == _model.FirstName && u.Email == _model.Email && u.PasswordQuestion == _model.PasswordQuestion && u.PaswordAnswer == _model.PaswordAnswer).FirstOrDefault();


                TempData["Message"] = Convert.ToString(sifreUnutan.Password);

            }
            else {
                 TempData["Message"] = "Tekrar Deneyiniz";
            }
           
            return View();

        }

        [UserAuthorize]
        public ActionResult Home()
        {
            return View(db.Posts.Where(p => p.IsDeleted == false).OrderByDescending(p => p.PostID).ToList());
        }

        [UserAuthorize]
        public ActionResult Posts(int id)
        {
            return View(db.Posts.Where(p => p.CategoryID == id).OrderByDescending(p => p.PostID).ToList());
        }
        [UserAuthorize]
        public ActionResult Post(int id)
        {
            return View(Tuple.Create<Post, List<PostImage>, Comment, List<Comment>>(db.Posts.Find(id), db.PostImages.Where(pi => pi.PostID == id).ToList(), new Comment(), db.Comments.Where(c => c.PostID == id).ToList()));
        }

        [UserAuthorize]
        [HttpPost]
        public JsonResult Post(int id, [Bind(Prefix = "Item3")]Comment item)
        {
            int uId = Convert.ToInt32(Session["id"]);
            item.UserID = uId;
            item.PostID = id;
            item.PublishDate = DateTime.Now;
            item.IsDeleted = false;
            db.Comments.Add(item);
            db.SaveChanges();
            List<CommentDTO> cList = db.Comments.Where(c => c.PostID == id).OrderByDescending(o => o.CommentID).Select(c => new CommentDTO
            {
                FirsName = c.User.FirstName,
                LastName = c.User.LastName,
                PublishDate = c.PublishDate,
                Text = c.Text
            }).ToList();

            return Json(cList, JsonRequestBehavior.AllowGet);
        }
        [UserAuthorize]
        public ActionResult Profil()
        {
            int id = Convert.ToInt32(Session["id"]);
            return View(db.Posts.Where(p => p.UserID == id).Where(p => p.IsDeleted == false).OrderByDescending(p => p.PostID).ToList());
        }

        public ActionResult Admin()
        {
            return View();
        }




	}
} 