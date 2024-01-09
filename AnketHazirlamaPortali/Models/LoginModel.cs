using System;
using System.ComponentModel.DataAnnotations;

namespace AnketHazirlamaPortali.Models
{
	public class LoginModel
	{
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }
    }
}

