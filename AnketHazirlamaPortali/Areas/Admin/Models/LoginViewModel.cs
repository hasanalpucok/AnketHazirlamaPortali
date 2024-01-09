﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AnketHazirlamaPortali.Areas.Admin.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }
    }
}
