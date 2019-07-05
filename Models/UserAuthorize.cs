using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCOneMagic.Models
{
    public class UserAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["oturum"] != null)
            {
                return true;
            }
            else
            {
                httpContext.Response.Redirect("/Home/Login");
                return false;
            }
        }
    }
}