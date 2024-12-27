namespace DepoYonetimSistemi.Models
{
    public class UrunDepo
    {
        public int ID { get; set; }
        public string Ad { get; set; }
        public int Fiyat { get; set; }
        public int StokDurumu { get; set; }
        public string DepoAdi { get; set; }

        public int DepoID { get; set; }
    }
}
