namespace DepoYonetimSistemi.Models
{
    public class Talep
    {
        public int TalepID { get; set; }
        public string UrunAdi { get; set; }
        public int Miktar { get; set; }

        public Depo Depo { get; set; }
    }
}
