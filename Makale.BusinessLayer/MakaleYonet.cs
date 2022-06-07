using Makale.DataAccessLayer;
using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.BusinessLayer
{
    public class MakaleYonet
    {
        private Repository<Not> repoNot = new Repository<Not>();
        BusinessLayerResult<Not> notResult = new BusinessLayerResult<Not>();
        public List<Not> MakaleGetir()
        {
            return repoNot.List();
        }
        public IQueryable<Not> MakaleGetirQueryable()
        {
            //bunun açıklamasına bak
            return repoNot.ListQueryable();
        }
        public Not MakaleBul(int id)
        {
            return repoNot.Find(x => x.Id == id);
        }

        public BusinessLayerResult<Not> MakaleKaydet(Not not)
        {
            notResult.Sonuc = repoNot.Find(x => x.Baslik == not.Baslik && x.KategoriId == not.KategoriId);
            if (notResult.Sonuc != null)
            {
                notResult.hata.Add("Bu makale kayıtlı.");
            }
            else
            {
                Not n = new Not();
                n.Kullanici = not.Kullanici;
                n.KategoriId = not.KategoriId;
                n.Baslik = not.Baslik;
                n.Icerik = not.Icerik;
                n.Taslak = not.Taslak;
                n.DegistirenKullanici = not.Kullanici.KullaniciAdi;
                int sonuc = repoNot.Insert(n);
                if (sonuc == 0)
                {
                    notResult.hata.Add("Makale kaydedilemedi.");
                }
                else
                {
                    notResult.Sonuc = n;
                }
            }
            return notResult;
        }

        public BusinessLayerResult<Not> MakaleUpdate(Not not)
        {
            notResult.Sonuc = repoNot.Find(x => x.Baslik == not.Baslik && x.KategoriId == not.KategoriId && x.Id != not.Id);
            if (notResult.Sonuc != null)
            {
                notResult.hata.Add("Bu makale kayıtlı.");
            }
            else
            {
                notResult.Sonuc = repoNot.Find(x => x.Id == not.Id);
                notResult.Sonuc = repoNot.Find(x => x.KategoriId == not.KategoriId);
                notResult.Sonuc = repoNot.Find(x => x.Baslik == not.Baslik);
                notResult.Sonuc = repoNot.Find(x => x.Icerik == not.Icerik);
                notResult.Sonuc = repoNot.Find(x => x.Taslak == not.Taslak);
                notResult.Sonuc = repoNot.Find(x => x.DegistirenKullanici == not.Kullanici.KullaniciAdi);

                int sonuc = repoNot.Update(notResult.Sonuc);
                if (sonuc == 0)
                {
                    notResult.hata.Add("Makale değiştirilemedi");
                }
                else
                {
                    notResult.Sonuc = repoNot.Find(x => x.Id == not.Id);
                }
            }
            return notResult;
        }

        public BusinessLayerResult<Not> MakaleSil(int id)
        {
            Not not = repoNot.Find(x => x.Id == id);
            if (not != null)
            {
                int sonuc = repoNot.Delete(not);
                if (sonuc == 0)
                {
                    notResult.hata.Add("Makale silinemedi.");
                }
            }
            else
            {
                notResult.hata.Add("Makale bulunamadı");
            }
            return notResult;
        }
    }
}
