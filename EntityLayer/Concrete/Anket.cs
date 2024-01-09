using System;
namespace EntityLayer.Concrete
{
	public class Anket
	{
        public int Id { get; set; }
        public string Baslik { get; set; }

        // Anketin soruları
        public List<Soru> Sorular { get; set; }
    }
}

