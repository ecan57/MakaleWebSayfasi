using Makale.DataAccessLayer;
using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.BusinessLayer
{
    public class Test
    {
        Repository<Kullanici> repoKullanici = new Repository<Kullanici>();
        Repository<Kategori> repoKategori = new Repository<Kategori>();
        public Test()
        {
            DatabaseContext db = new DatabaseContext();
            db.Kullanicilar.ToList();
            //üsttekinin yerine alttakide kullanılabilirmiş
            //db.Database.CreateIfNotExists();

        }
        public void InsertTest()
        {
            repoKullanici.Insert(new Kullanici()
            {
                Adi = "xx",
                Soyadi = "yy",
                Email = "xx@yy.com",
                KullaniciAdi = "zz",
                Sifre = "123",
                Aktif = true,
                Admin = true,
                AktifGuid = Guid.NewGuid(),
                KayitTarihi = DateTime.Now,
                DegistirmeTarihi = DateTime.Now.AddMinutes(5),
                DegistirenKullanici = "samet"
            });
        }
        public void UpdateTest()
        {
            Kullanici kullanici = repoKullanici.Find(x => x.Adi == "xx");
            if (kullanici != null)
            {
                kullanici.Adi = "Ömer";
                repoKullanici.Save();
            }
        }
        public void DeleteTest()
        {
            Kullanici kullanici = repoKullanici.Find(x => x.Adi == "Ömer");
            if (kullanici != null)
            {
                repoKullanici.Delete(kullanici);
            }
        }
        Repository<Yorum> repoYorum = new Repository<Yorum>();
        Repository<Not> repoNot = new Repository<Not>();
        public void YorumTest()
        {
            Kullanici kullanici = repoKullanici.Find(x => x.Id == 1);
            Not makale = repoNot.Find(x => x.Id == 3);

            Yorum yorum = new Yorum()
            {
                YorumMetni = "Bu bir test yorumudur.",
                KayitTarihi = DateTime.Now,
                DegistirmeTarihi = DateTime.Now,
                DegistirenKullanici = "semih",
                Kullanici = kullanici
            };
            repoYorum.Insert(yorum);
        }
    }
}
