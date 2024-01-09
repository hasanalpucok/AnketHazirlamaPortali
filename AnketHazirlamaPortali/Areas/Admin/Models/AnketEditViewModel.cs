using System;
using EntityLayer.Concrete;

namespace AnketHazirlamaPortali.Areas.Admin.Models
{
    public class AnketEditViewModel
    {
        public int AnketId { get; set; }
        public string AnketBaslik { get; set; }
        public List<SoruViewModel> Sorular { get; set; }
    }

}

