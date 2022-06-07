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
using MakaleWebProjesi.Filter;
using MakaleWebProjesi.Models;

namespace MakaleWebProjesi.Controllers
{
    [Exc]
    [Auth]
    public class MakaleController : Controller
    {
        MakaleYonet makaleYonet = new MakaleYonet();
        KategoriYonet kategoriYonet = new KategoriYonet();
        // GET: Makale
        public ActionResult Index()
        {
            //var nots = makaleYonet.MakaleGetir();
            Kullanici kullanici = (Kullanici)Session["login"];
            var nots = makaleYonet.MakaleGetirQueryable().Include("Kullanici").Where(x => x.Kullanici.Id == kullanici.Id).OrderByDescending(x => x.DegistirmeTarihi);
            return View(nots.ToList());
        }
        public ActionResult Begendiklerim()
        {
            BegeniYonet begeniYonet = new BegeniYonet();
            Kullanici kullanici = Session["login"] as Kullanici;
            var makale = begeniYonet.ListQueryable().Include("Kullanici").Include("Makale").Where(x => x.Kullanici.Id == kullanici.Id).Select(x=>x.Makale).Include("Kategori").Include("Kullanici").OrderByDescending(x => x.DegistirmeTarihi);
            return View("Index", makale.ToList());

        }
        // GET: Makale/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Not not = makaleYonet.MakaleBul(id.Value);
            if (not == null)
            {
                return HttpNotFound();
            }
            return View(not);
        }
        // GET: Makale/Create
        public ActionResult Create()
        {
            //ViewBag.KategoriId = new SelectList(kategoriYonet.KategoriGetir(), "Id", "Baslik");
            //Kategori yönetler yerine cachehelper kullandık. alttaki metotlardada aynılarını yaptık.
            ViewBag.KategoriId = new SelectList(CacheHelper.KategoriCache(), "Id", "Baslik");
            return View();
        }
        // POST: Makale/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Not not)
        {
            ModelState.Remove("KayitTarihi");
            ModelState.Remove("DegistirmeTarihi");
            ModelState.Remove("DegistirenKullanici");

            ViewBag.KategoriId = new SelectList(CacheHelper.KategoriCache(), "Id", "Baslik", not.KategoriId);

            if (ModelState.IsValid)
            {
                not.Kullanici = (Kullanici)Session["login"];
                BusinessLayerResult<Not> sonuc = makaleYonet.MakaleKaydet(not);
                if (sonuc.hata.Count > 0)
                {
                    sonuc.hata.ForEach(x => ModelState.AddModelError("", x));
                    return View(not);
                }
                return RedirectToAction("Index");
            }
            return View(not);
        }
        // GET: Makale/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Not not = makaleYonet.MakaleBul(id.Value);
            if (not == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriId = new SelectList(CacheHelper.KategoriCache(), "Id", "Baslik", not.KategoriId);
            return View(not);
        }
        // POST: Makale/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Not not)
        {
            ModelState.Remove("KayitTarihi");
            ModelState.Remove("DegistirmeTarihi");
            ModelState.Remove("DegistirenKullanici");

            ViewBag.KategoriId = new SelectList(CacheHelper.KategoriCache(), "Id", "Baslik", not.KategoriId);

            if (ModelState.IsValid)
            {
                not.Kullanici = (Kullanici)Session["login"];
                BusinessLayerResult<Not> sonuc = makaleYonet.MakaleKaydet(not);
                if (sonuc.hata.Count > 0)
                {
                    sonuc.hata.ForEach(x => ModelState.AddModelError("", x));
                    return View(not);
                }
                return RedirectToAction("Index");
            }
            return View(not);
        }
        // GET: Makale/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Not not = makaleYonet.MakaleBul(id.Value);
            if (not == null)
            {
                return HttpNotFound();
            }
            return View(not);
        }
        // POST: Makale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BusinessLayerResult<Not> sonuc = makaleYonet.MakaleSil(id);
            return RedirectToAction("Index");
        }
        
        BegeniYonet begeniYonet = new BegeniYonet();
        public ActionResult MakaleBegen(int[] dizi)
        {
            Kullanici kullanici = Session["login"] as Kullanici;
            List<int> dizibegeni = new List<int>();

            if (kullanici != null)
            {
                if (dizi != null)
                {
                    dizibegeni = begeniYonet.List(x => x.Kullanici.Id == kullanici.Id && dizi.Contains(x.Makale.Id)).Select(x => x.Makale.Id).ToList();
                    //üstteki kodun yaptığı işlem:
                    //Select MakaleId from begeni b where b.kullaniciid=kullanici.ıd and b.makaleid in(3,5,9)
                }
            }
            return Json(new { sonuc = dizibegeni });
        }
        [HttpPost]
        public ActionResult LikeDurumuUpdate(int notid, bool like)
        {
            int sonuc = 0;
            Kullanici kullanici = Session["login"] as Kullanici;
            Begeni begeni = begeniYonet.BegeniGetir(notid, kullanici.Id);
            Not not = makaleYonet.MakaleBul(notid);
            if (begeni != null && like == false)
            {
                //delete
                sonuc = begeniYonet.BegeniSil(begeni);
            }
            else if (begeni == null && like == true)
            {
                //insert
                sonuc = begeniYonet.BegeniEkle(new Begeni()
                {
                    Kullanici = kullanici,
                    Makale = not
                });
            }
            if (sonuc > 0)
            {
                if (like)
                {
                    not.BegeniSayisi++;
                }
                else
                {
                    not.BegeniSayisi--;
                }
                BusinessLayerResult<Not> result = makaleYonet.MakaleUpdate(not);
                if (result.hata.Count == 0)
                {
                    return Json(new { hata = false, sonuc = not.BegeniSayisi });
                }
            }
            return Json(new { hata = true, sonuc = not.BegeniSayisi });
        }
    }
}
