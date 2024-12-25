namespace DepoYonetimSistemi.Models
{
    public class Urun
    {
        public int ID { get; set; }
        public string Ad { get; set; }
        public int Fiyat{ get; set; }
        public int StokDurumu { get; set; }
        public int DepoID { get; set; }
        public Depo Depo { get; set; }
    }
}
