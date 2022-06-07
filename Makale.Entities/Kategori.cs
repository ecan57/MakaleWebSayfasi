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
    [Table("Kategoriler")]
    public class Kategori: EntityBase
    {
        [Required, StringLength(50), DisplayName("Kategori")]
        public string Baslik { get; set; }
        [StringLength(50), DisplayName("Açıklama")]
        public string Aciklama { get; set; }

        public virtual List<Not> Makaleler { get; set; }
        public virtual List<Yorum> Yorumlar { get; set; }
        //virtual ilişkili sınıflar için kullanılıyormuş. Bir makalenin bir kategorisi olur.

        public Kategori()
        {
            Makaleler = new List<Not>();
        }
    }
}
