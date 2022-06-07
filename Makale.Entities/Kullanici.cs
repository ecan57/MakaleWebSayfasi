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
    [Table("Kullanıcılar")]
    public class Kullanici: EntityBase
    {
        [StringLength(25), DisplayName("Adı")]
        public string Adi { get; set; }
        [StringLength(25), DisplayName("Soyadı")]
        public string Soyadi { get; set; }
        [Required, StringLength(25), DisplayName("Kullanıcı Adı")]
        public string KullaniciAdi { get; set; }
        [Required, StringLength(50), DisplayName("E-Posta")]
        public string Email { get; set; }
        [Required, StringLength(25), DisplayName("Şifre")]
        public string Sifre { get; set; }
        [StringLength(30), ScaffoldColumn(false), DisplayName("Profil Resmi")] //kullanıcı kontroller oluştururken eklersek scafoldingcolumn u index.cshtmlde bulunmayacaktır. tek tek elle silmemizi önler.
        public string ProfilResmi { get; set; }
        public bool Admin { get; set; }
        public bool Aktif { get; set; }
        [ScaffoldColumn(false)]
        public Guid AktifGuid { get; set; }

        
        public virtual List<Not> Makaleler { get; set; }
        public virtual List<Yorum> Yorumlar { get; set; }
        public virtual List<Begeni> Begeniler { get; set; }
    }
}
