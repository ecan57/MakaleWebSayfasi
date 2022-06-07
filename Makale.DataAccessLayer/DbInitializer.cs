using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.DataAccessLayer
{
    public class DbInitializer: CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed (DatabaseContext context)
        {
            Kullanici admin = new Kullanici()
            {
                Adi = "Emine",
                Soyadi = "CAN",
                Email = "ecan.projeler@gmail.com",
                Aktif = true,
                Admin = true,
                KullaniciAdi = "Admin",
                Sifre = "1234",
                AktifGuid = Guid.NewGuid(),
                KayitTarihi = DateTime.Now,
                ProfilResmi = "profilfotografi.jpg",
                DegistirmeTarihi = DateTime.Now.AddMinutes(5),
                DegistirenKullanici = "Admin"
            };
            context.Kullanicilar.Add(admin);
            context.SaveChanges();

            for (int i = 1; i < 10; i++)
            {
                Kullanici user = new Kullanici()
                {
                    Adi = FakeData.NameData.GetFirstName(),
                    Soyadi = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    AktifGuid = Guid.NewGuid(),
                    Aktif = true,
                    Admin = false,
                    KullaniciAdi = $"user{i}",
                    Sifre = "123",
                    KayitTarihi = DateTime.Now.AddDays(-1),
                    ProfilResmi = "profilfotografi.jpg",
                    DegistirmeTarihi = DateTime.Now,
                    DegistirenKullanici = $"user{i}",
                    
                };
                context.Kullanicilar.Add(user);
            }
            context.SaveChanges();

            List<Kullanici> klist = context.Kullanicilar.ToList();

            //Kategori verileri ekleniyor.
            for (int i = 1; i < 10; i++)
            {
                Kategori kategori = new Kategori()
                {
                    Baslik = FakeData.PlaceData.GetStreetName(),
                    Aciklama = FakeData.PlaceData.GetAddress(),
                    KayitTarihi = DateTime.Now,
                    DegistirmeTarihi = DateTime.Now,
                    DegistirenKullanici = "elif"
                };
                context.Kategoriler.Add(kategori);

                //Kategoriye makale ekleniyor.
                for (int j = 0; j < 6; j++)
                {
                    Not not = new Not()
                    {
                        Baslik = FakeData.PlaceData.GetStreetName(),
                        Icerik = FakeData.TextData.GetSentences(3),
                        Taslak = false,
                        BegeniSayisi = FakeData.NumberData.GetNumber(1, 9),
                        Kategori = kategori,
                        KayitTarihi = DateTime.Now.AddDays(-2),
                        DegistirmeTarihi = DateTime.Now,
                        Kullanici = klist[j],
                        DegistirenKullanici = klist[j].KullaniciAdi
                    };
                    kategori.Makaleler.Add(not);

                    //Makaleye yorum ekleniyor.
                    for (int k = 0; k < 3; k++)
                    {
                        Yorum yorum = new Yorum()
                        {
                            YorumMetni = FakeData.TextData.GetSentence(),
                            KayitTarihi = DateTime.Now,
                            DegistirmeTarihi = DateTime.Now,
                            Kullanici = klist[FakeData.NumberData.GetNumber(1, 9)],
                            DegistirenKullanici = klist[FakeData.NumberData.GetNumber(1, 9)].KullaniciAdi
                        };
                        not.Yorumlar.Add(yorum);
                    }

                    //Makaleye beğeni ekleniyor.
                    for (int x = 0; x < not.BegeniSayisi; x++)
                    {
                        Begeni begeni = new Begeni()
                        {
                            Kullanici = klist[FakeData.NumberData.GetNumber(1, 9)],
                            Makale = not
                        };
                        not.Begeniler.Add(begeni);
                    }
                }
            }
            context.SaveChanges();
        }
    }
}
