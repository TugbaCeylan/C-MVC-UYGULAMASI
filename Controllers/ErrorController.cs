using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCOneMagic.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Yetkisiz()
        {
            return View();
        }
	}
}