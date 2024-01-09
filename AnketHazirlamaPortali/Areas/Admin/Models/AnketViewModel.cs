using System;
using System.ComponentModel.DataAnnotations;

namespace AnketHazirlamaPortali.Areas.Admin.Models
{
    public class AnketViewModel
    {
        [Required(ErrorMessage = "Başlık alanı boş bırakılamaz.")]
        public string Baslik { get; set; }

        [Required(ErrorMessage = "En az bir soru eklemelisiniz.")]
        public List<string> Sorular { get; set; }
    }
}

