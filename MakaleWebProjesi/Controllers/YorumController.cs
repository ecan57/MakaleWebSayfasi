using Makale.BusinessLayer;
using Makale.Entities;
using MakaleWebProjesi.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MakaleWebProjesi.Controllers
{
    [Exc]
    public class YorumController : Controller
    {
        // GET: Yorum
        public ActionResult YorumGoster(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MakaleYonet makaleYonet = new MakaleYonet();
            Not not = makaleYonet.MakaleBul(id.Value);
            if (not == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialYorum", not.Yorumlar);
        }
        YorumYonet yorumYonet = new YorumYonet();
        [Auth]
        [HttpPost]
        public ActionResult YorumUpdate(int? id, string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yorum yorum = yorumYonet.YorumBul(id.Value);
            if (yorum == null)
            {
                return new HttpNotFoundResult();
            }
            yorum.YorumMetni = text;

            if (yorumYonet.YorumUpdate(yorum) > 0)
            {
                return Json(new { sonuc = true });
            }
            return Json(new { sonuc = false });
        }
        //Json kullanınca post ta ajax isteğine izin veriyor ama get de izin vermesi için AllowGet demeliyiz.
        //get ler ajax isteğine kapalıdır. güvenlik açısından izin vermezmiş.post saldırısını engellemek için.
        [Auth]
        public ActionResult YorumSil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yorum yorum = yorumYonet.YorumBul(id.Value);
            if (yorum == null)
            {
                return new HttpNotFoundResult();
            }
            if (yorumYonet.YorumSil(yorum) > 0)
            {
                return Json(new { sonuc = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { sonuc = false }, JsonRequestBehavior.AllowGet);
        }
        MakaleYonet makaleYonet1 = new MakaleYonet();
        [Auth]
        [HttpPost]
        public ActionResult YorumEkle(Yorum yorum, int? notid)
        {
            if (notid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Not not = makaleYonet1.MakaleBul(notid.Value);
            
            if (not == null)
            {
                return new HttpNotFoundResult();
            }

            yorum.Makale = not;
            
            yorum.Kullanici = Session["login"] as Kullanici;

            if (yorumYonet.YorumEkle(yorum) > 0)
            {
                return Json(new { sonuc = true });
            }
            return Json(new { sonuc = false });
        }
    }
}