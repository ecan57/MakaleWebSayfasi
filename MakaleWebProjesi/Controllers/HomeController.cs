using Makale.BusinessLayer;
using Makale.Entities;
using Makale.Entities.ViewModels;
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
    public class HomeController : Controller
    {
        MakaleYonet makaleYonet = new MakaleYonet();
        KategoriYonet kategoriYonet = new KategoriYonet();
        KullaniciYonet kullaniciYonet = new KullaniciYonet();
        // GET: Home
        public ActionResult Index()
        {
            //testleri buraya yazıyoruz.
            Test test = new Test();
            //test.InsertTest();
            //test.UpdateTest();
            //test.DeleteTest();
            //test.YorumTest();
            //return View();

            //int sayi = 0, sayi2 = 5;
            //int sonuc = sayi2 / sayi;
            ///Üstteki kodu hata mesajı almak için yazdık.

            //return View(makaleYonet.MakaleGetir().OrderByDescending(x=>x.DegistirmeTarihi).ToList());

            return View(makaleYonet.MakaleGetirQueryable().Where(x => x.Taslak == false).OrderByDescending(x => x.DegistirmeTarihi).ToList());

        }
       
        public ActionResult Kategori(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Kategori kategori = kategoriYonet.KategoriBul(id.Value);

            if (kategori == null)
            {
                return HttpNotFound();
            }
            List<Not> makaleler =makaleYonet.MakaleGetirQueryable().Where(x=>x.Taslak == false && x.KategoriId == id).OrderByDescending(x => x.DegistirmeTarihi).ToList();
            return View("Index", makaleler);
        }
        public ActionResult EnBegenilenler()
        {
            return View("Index", makaleYonet.MakaleGetir().OrderByDescending(x => x.BegeniSayisi).ToList());
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Kullanici> sonuc = kullaniciYonet.LoginKullanici(model);
                if (sonuc.hata.Count > 0)
                {
                    sonuc.hata.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }
                Session["login"] = sonuc.Sonuc;
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Kullanici> result = kullaniciYonet.KullaniciKaydet(model);
                if (result.hata.Count > 0)
                {
                    result.hata.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }
                return RedirectToAction("RegisterOK");
            }
            return View();
        }
        public ActionResult RegisterOK()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
        [Auth]
        public ActionResult ProfilGoster()
        {
            Kullanici kullanici = Session["login"] as Kullanici;
            BusinessLayerResult<Kullanici> result = kullaniciYonet.KullaniciBul(kullanici.Id);
            if (result.hata.Count > 0)
            {
                //hata sayfasına yönledndiri
            }
            return View(result.Sonuc);
        }
        [Auth]
        public ActionResult ProfilDuzenle()
        {
            Kullanici kullanici = Session["login"] as Kullanici;
            BusinessLayerResult<Kullanici> result = kullaniciYonet.KullaniciBul(kullanici.Id);
            if (result.hata.Count > 0)
            {
                //hata sayfasına yönledndiri
            }
            return View(result.Sonuc);
        }
        [Auth]
        [HttpPost]
        public ActionResult ProfilDuzenle(Kullanici model, HttpPostedFileBase profilResmi)
        {
            ModelState.Remove("DegistirenKullanici");
            if (ModelState.IsValid)
            {
                if (profilResmi != null && (profilResmi.ContentType == "image/jpeg" || profilResmi.ContentType == "image/jpg" || profilResmi.ContentType == "image/png" || profilResmi.ContentType == "image/gif"))
                {
                    string dosyaAdi = $"kullanici_{model.Id}.{profilResmi.ContentType.Split('/')[1]}";
                    profilResmi.SaveAs(Server.MapPath($"~/Images/{dosyaAdi}"));
                    model.ProfilResmi = dosyaAdi;
                }
                BusinessLayerResult<Kullanici> sonuc = kullaniciYonet.KullaniciUpdate(model);
                if (sonuc.hata.Count > 0)
                {
                    for (int i = 0; i < sonuc.hata.Count; i++)
                    {
                        ModelState.AddModelError("", (sonuc.hata)[i]);
                    }
                    return View(model);
                }
                Session["login"] = sonuc.Sonuc;
                return RedirectToAction("ProfilGoster");
            }
            return View(model);
        }
        [Auth]
        public ActionResult ProfilSil()
        {
            Kullanici kullanici = Session["login"] as Kullanici;
            BusinessLayerResult<Kullanici> result = kullaniciYonet.KullaniciSil(kullanici.Id);
            if (result.hata.Count > 0)
            {
                return RedirectToAction("ProfilGoster");
            }
            Session.Clear();
            return RedirectToAction("Index");
        }
        [Auth]
        public ActionResult UserActivate(Guid id)
        {
            BusinessLayerResult<Kullanici> sonuc = kullaniciYonet.ActivateUser(id);
            if (sonuc.hata.Count > 0)
            {
                TempData["error"] = sonuc.hata;
                return RedirectToAction("UserActivateError");
            }
            return RedirectToAction("UserActivateOK");
        }
        public ActionResult UserActivateError()
        {
            List<string> hataMesaj = null;
            if (TempData["error"] != null)
            {
                hataMesaj =TempData["error"] as List<string>;
            }
            return View(hataMesaj);
        }
        public ActionResult UserActivateOK()
        {
            return View();
        }
        public ActionResult YetkisizErisim()
        {
            return View();
        }
        public ActionResult HataSayfasi()
        {
            return View();
        }
    }
}