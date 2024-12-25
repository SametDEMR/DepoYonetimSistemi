namespace DepoYonetimSistemi.Models
{
    public class Depo
    {
        public int DepoID { get; set; }
        public string DepoAdi { get; set; }
        public string DepoKonumu  { get; set; }

        public ICollection<Urun> urunler { get; set; }
        public ICollection<Talep> talepler { get; set; }
    }
}
