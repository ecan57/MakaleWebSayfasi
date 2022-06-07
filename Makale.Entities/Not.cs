using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.Entities
{
    [Table("Makaleler")]
    public class Not: EntityBase
    {
        [Required, StringLength(60), DisplayName("Başlık")]
        public string Baslik { get; set; }
        [Required, StringLength(1000), DisplayName("İçerik")]
        public string Icerik { get; set; }
        public bool Taslak { get; set; }
        [DisplayName("Beğeni Sayısı")]
        public int BegeniSayisi { get; set; }
        public int KategoriId { get; set; }

        public virtual Kategori Kategori { get; set; }
        public virtual Kullanici Kullanici { get; set; }
        public virtual List<Yorum> Yorumlar { get; set; }//Bir makalenin birden fazla yorumu olabilir. (List)
        public virtual List<Begeni> Begeniler { get; set; }

        public Not()
        {
            Yorumlar = new List<Yorum>();
            Begeniler = new List<Begeni>();
        }
    }
}
