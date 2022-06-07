using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.Entities
{
    [Table("Yorumlar")]
    public class Yorum: EntityBase
    {
        [Required, StringLength(300)]
        public string YorumMetni { get; set; }

        public virtual Not Makale { get; set; }
        public virtual Kullanici Kullanici { get; set; }
    }
}
