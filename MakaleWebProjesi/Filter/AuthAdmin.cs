using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MakaleWebProjesi.Filter
{
    public class AuthAdmin : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            Kullanici kullanici = filterContext.HttpContext.Session["login"] as Kullanici;
            if (kullanici != null && kullanici.Admin == false)
            {
                filterContext.Result = new RedirectResult("/Home/YetkisizErisim");
            }
        }
    }
}