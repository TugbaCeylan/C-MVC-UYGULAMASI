using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCOneMagic.Models
{
    public class AdminAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["oturum"] != null)
            {
                User girisYapan = (User)httpContext.Session["oturum"];
                if (girisYapan.RoleID == 1)
                {
                    return true;
                }
                httpContext.Response.Redirect("/Error/Yetkisiz");
                return false;
            }
            else
            {
                httpContext.Response.Redirect("/Home/Login");
                return false;
            }
        }
    }
}