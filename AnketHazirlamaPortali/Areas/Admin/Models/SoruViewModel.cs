using System;
using System.ComponentModel.DataAnnotations;

namespace AnketHazirlamaPortali.Areas.Admin.Models
{
    public class SoruViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Soru metni zorunludur.")]
        public string Metin { get; set; }
    }
}

