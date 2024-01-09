using System;
namespace EntityLayer.Concrete
{
    public class Soru
    {
        public int Id { get; set; }
        public string Metin { get; set; }

        public int AnketId { get; set; }
        public Anket Anket { get; set; }

        public List<Cevap> Cevaplar { get; set; }
    }
}

