using System;
namespace EntityLayer.Concrete
{
    public class Cevap
    {
        public int Id { get; set; }
        public string CevapMetni { get; set; }

        // Cevaplayan kullanıcı
        public string UserId { get; set; }
        public AppUser User { get; set; }

        // Hangi ankete verildiği
        public int AnketId { get; set; }
        public Anket Anket { get; set; }

        // Hangi soruya verildiği
        public int SoruId { get; set; }
        public Soru Soru { get; set; }
    }
}

