using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makale.Common;
using Makale.DataAccessLayer;
using Makale.Entities;
using Makale.Entities.ViewModels;

namespace Makale.BusinessLayer
{
    public class KullaniciYonet
    {
        private Repository<Kullanici> repoKullanici = new Repository<Kullanici>();
        BusinessLayerResult<Kullanici> kullaniciSonuc = new BusinessLayerResult<Kullanici>();
        public BusinessLayerResult<Kullanici> KullaniciKaydet(RegisterViewModel model)
        {
            //Kullanıcı Adı ve E-Posta kontrolü
            //Kayıt İşlemi
            //Aktivasyon maili gönderme
            Kullanici kullanici = repoKullanici.Find(x => x.KullaniciAdi == model.KullaniciAdi || x.Email == model.Email);

            if (kullanici != null)
            {
                if (kullanici.KullaniciAdi == model.KullaniciAdi)
                {
                    kullaniciSonuc.hata.Add("Kullanıcı adı kayıtlı.");
                }
                if (kullanici.Email == model.Email)
                {
                    kullaniciSonuc.hata.Add("E-posta adresi kayıtlı.");
                }
            }
            else
            {
                int sonuc = repoKullanici.Insert(new Kullanici() 
                { 
                    KullaniciAdi = model.KullaniciAdi,
                    Email = model.Email,
                    Sifre = model.Sifre,
                    AktifGuid = Guid.NewGuid(),
                    Aktif = false,
                    Admin = false
                });
                if (sonuc > 0)
                {
                    kullaniciSonuc.Sonuc = repoKullanici.Find(x => x.Email == model.Email && x.KullaniciAdi == model.KullaniciAdi);
                    //Aktivasyon maili gönderilecek
                    string siteUrl = ConfigHelper.Get<string>("SiteRootUrl");
                    string activateURL = $"{siteUrl}/Home/UserActivate/{kullaniciSonuc.Sonuc.AktifGuid}";
                    string body = $"Merhaba {kullaniciSonuc.Sonuc.Adi}{kullaniciSonuc.Sonuc.Soyadi} <br> Hesabınızı aktifleştirmek için, <a href='{activateURL}' target='_blank'>tıklayınız </a>"; //_blank yeni sekmede aç demek
                    MailHelper.SendMail(body, kullaniciSonuc.Sonuc.Email, "Hesap Aktifleştirme");
                }
            }
            return kullaniciSonuc;
        }
        public BusinessLayerResult<Kullanici> LoginKullanici(LoginViewModel model)
        {
            kullaniciSonuc.Sonuc = repoKullanici.Find(x => x.KullaniciAdi == model.KullaniciAdi && x.Sifre == model.Sifre);
            if (kullaniciSonuc.Sonuc != null)
            {
                if (!kullaniciSonuc.Sonuc.Aktif)
                {
                    kullaniciSonuc.hata.Add("Kullanıcı aktifleştirilmemiştir. Lütfen e-posta adresinizi kontrol ediniz.");
                }
            }
            else
            {
                kullaniciSonuc.hata.Add("Kullanıcı adı ya da şifre uyuşmuyor.");
            }
            return kullaniciSonuc;
        }

        public BusinessLayerResult<Kullanici> ActivateUser(Guid aktifGuid)
        {
            kullaniciSonuc.Sonuc = repoKullanici.Find(x => x.AktifGuid == aktifGuid);
            if (kullaniciSonuc.Sonuc != null)
            {
                if (kullaniciSonuc.Sonuc.Aktif)
                {
                    kullaniciSonuc.hata.Add("Kullanıcı zaten aktif edilmiştir");
                    return kullaniciSonuc;
                }
                kullaniciSonuc.Sonuc.Aktif = true;
                repoKullanici.Update(kullaniciSonuc.Sonuc);
            }
            return kullaniciSonuc;
        }

        public BusinessLayerResult<Kullanici> KullaniciBul(int id)
        {
            kullaniciSonuc.Sonuc = repoKullanici.Find(x => x.Id == id);
            if (kullaniciSonuc.Sonuc == null)
            {
                kullaniciSonuc.hata.Add("Kullanıcı bulunamadı.");
            }
            return kullaniciSonuc;
        }

        public BusinessLayerResult<Kullanici> KullaniciUpdate(Kullanici model)
        {
            Kullanici user = repoKullanici.Find(x => x.Id == model.Id && (x.KullaniciAdi == model.KullaniciAdi || x.Email == model.Email));
            if (user != null && user.Id != model.Id)
            {
                if (user.KullaniciAdi == model.KullaniciAdi)
                {
                    kullaniciSonuc.hata.Add("Bu kullanıcı adı kayıtlıdır.");
                }
                if (user.Email == model.Email)
                {
                    kullaniciSonuc.hata.Add("Bu e-posta kayıtlıdır.");
                }
                return kullaniciSonuc;
            }
            kullaniciSonuc.Sonuc = repoKullanici.Find(x => x.Id == model.Id);

            kullaniciSonuc.Sonuc.Email = model.Email;
            kullaniciSonuc.Sonuc.Adi = model.Adi;
            kullaniciSonuc.Sonuc.Soyadi = model.Soyadi;
            kullaniciSonuc.Sonuc.Sifre = model.Sifre;

            if (string.IsNullOrEmpty(model.ProfilResmi) == false)
            {
                kullaniciSonuc.Sonuc.ProfilResmi = model.ProfilResmi;
            }
            if (repoKullanici.Update(kullaniciSonuc.Sonuc) == 0)
            {
                kullaniciSonuc.hata.Add("Kullanıcı güncellenemedi.");
            }
            return kullaniciSonuc;
        }

        public BusinessLayerResult<Kullanici> KullaniciSil(int id)
        {
            Kullanici kullanici = repoKullanici.Find(x => x.Id == id);
            if (kullanici == null)
            {
                kullaniciSonuc.hata.Add("Kullanıcı bulunamadı.");
            }
            else
            {
                if (repoKullanici.Delete(kullanici) == 0)
                {
                    kullaniciSonuc.hata.Add("Kullanıcı silinemedi.");
                }
            }
            return kullaniciSonuc;
        }


        public List<Kullanici> KullaniciGetir()
        {
            return repoKullanici.List();
        }
    }
}
