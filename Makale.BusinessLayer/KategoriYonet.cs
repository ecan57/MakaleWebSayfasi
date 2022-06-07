using Makale.DataAccessLayer;
using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.BusinessLayer
{
    public class KategoriYonet
    {
        private Repository<Kategori> repoKategori = new Repository<Kategori>();
        public List<Kategori> KategoriGetir()
        {
            return repoKategori.List();
        }
        public Kategori KategoriBul(int id)
        {
            return repoKategori.Find(x => x.Id == id);
        }
        BusinessLayerResult<Kategori> kategoriSonuc = new BusinessLayerResult<Kategori>();
        public BusinessLayerResult<Kategori> KategoriKaydet(Kategori model)
        {
           Kategori kategori = repoKategori.Find(x => x.Baslik == model.Baslik);

            if (kategori != null)
            {
                if (kategori.Baslik == model.Baslik)
                {
                    kategoriSonuc.hata.Add("Kategori adı kayıtlı.");
                }
            }
            else
            {
                int sonuc = repoKategori.Insert(new Kategori()
                {
                    Baslik = model.Baslik,
                    Aciklama = model.Aciklama
                });
                
            }
            return kategoriSonuc;
        }

        public BusinessLayerResult<Kategori> KategoriUpdate(Kategori model)
        {
            Kategori kategori = repoKategori.Find(x => x.Baslik == model.Baslik && x.Id != model.Id);
            
            if (kategori != null)
            {
                kategoriSonuc.hata.Add("Kategori adı kayıtlı.");
            }
            else
            {
                kategoriSonuc.Sonuc = repoKategori.Find(x => x.Id == model.Id);

                kategoriSonuc.Sonuc.Baslik = model.Baslik;
                kategoriSonuc.Sonuc.Aciklama = model.Aciklama;

                int sonuc = repoKategori.Update(kategoriSonuc.Sonuc);

                if (sonuc > 0)
                {
                    kategoriSonuc.Sonuc = repoKategori.Find(x => x.Id == model.Id);
                }
            }
            return kategoriSonuc;
        }
        public BusinessLayerResult<Kategori> KategoriSil(int id)
        {
            Kategori kategori = repoKategori.Find(x => x.Id == id);
            //kategorinin notlarını bul
            //notların yorumlarını bul
            //notların like bul dil
            //notu sil
            //kategori sil
            ///üstteki herbir silmeye kod yazmamak için diagramlarda foreign keylerin propertilerindeki insertte cascade yaptık.
            if (kategori == null)
            {
                kategoriSonuc.hata.Add("Kategori bulunamadı.");
            }
            int sonuc = repoKategori.Delete(kategori);
            return kategoriSonuc;
        }
    }
}
