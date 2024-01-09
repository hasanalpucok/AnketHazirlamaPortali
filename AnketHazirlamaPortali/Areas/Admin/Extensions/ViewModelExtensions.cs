using System;
using AnketHazirlamaPortali.Areas.Admin.Models;
using EntityLayer.Concrete;

namespace AnketHazirlamaPortali.Areas.Admin.Extensions
{
    public static class ViewModelExtensions
    {
        public static Soru ToEntity(this SoruViewModel soruViewModel)
        {
            return new Soru
            {
                Id = soruViewModel.Id,
                Metin = soruViewModel.Metin
            };
        }
    }

}

