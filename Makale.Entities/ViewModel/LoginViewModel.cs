using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Makale.Entities.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "{0} alanı boş geçilemez."), DisplayName("Kullanıcı Adı")]
        public string KullaniciAdi { get; set; }
        [Required(ErrorMessage = "{0} alanı boş geçilemez."), DisplayName("Şifre"), DataType(DataType.Password)]
        public string Sifre { get; set; }
    }
}