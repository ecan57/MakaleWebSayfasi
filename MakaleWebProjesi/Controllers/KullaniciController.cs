using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Makale.BusinessLayer;
using Makale.Entities;
using Makale.Entities.ViewModels;
using MakaleWebProjesi.Filter;
using MakaleWebProjesi.Models;

namespace MakaleWebProjesi.Controllers
{
    [Exc]
    [Auth][AuthAdmin]
    public class KullaniciController : Controller
    {
        KullaniciYonet kullaniciYonet = new KullaniciYonet();

        // GET: Kullanici
        public ActionResult Index()
        {
            return View(kullaniciYonet.KullaniciGetir());
        }

        // GET: Kullanici/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessLayerResult<Kullanici> sonuc = kullaniciYonet.KullaniciBul(id.Value);
            if (sonuc.Sonuc == null)
            {
                return HttpNotFound();
            }
            return View(sonuc.Sonuc);
        }

        // GET: Kullanici/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterViewModel kullanici)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Kullanici> result = kullaniciYonet.KullaniciKaydet(kullanici);
                if (result.hata.Count > 0)
                {
                    result.hata.ForEach(x => ModelState.AddModelError("", x));
                    return View(kullanici);
                }
                return RedirectToAction("Kullanici");
            }
            return View();
        }

        // GET: Kullanici/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessLayerResult<Kullanici> kullanici = kullaniciYonet.KullaniciBul(id.Value);
            if (kullanici == null)
            {
                return HttpNotFound();
            }
            return View(kullanici);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Kullanici> sonuc = kullaniciYonet.KullaniciUpdate(kullanici);
                if (sonuc.hata.Count > 0)
                {
                    sonuc.hata.ForEach(x => ModelState.AddModelError("", x));
                    return View(kullanici);
                }
                CacheHelper.CacheTemizle();
                return RedirectToAction("Index");
            }
            return View(kullanici);
        }

        // GET: Kullanici/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessLayerResult<Kullanici> kullanici = kullaniciYonet.KullaniciBul(id.Value);
            if (kullanici == null)
            {
                return HttpNotFound();
            }
            return View(kullanici);
        }

        // POST: Kullanici/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BusinessLayerResult<Kullanici> kullanici = kullaniciYonet.KullaniciBul(id);
            var sonuc = kullaniciYonet.KullaniciSil(id);
            if (sonuc.hata.Count > 0)
            {
                return View(kullanici);
            }
            CacheHelper.CacheTemizle();
            return RedirectToAction("Index");
        }
    }
}
