namespace DepoYonetimSistemi.Models
{
    public class Kullanici
    {
        public int ID { get; set; }
        public string Isim { get; set; }
        public string Soyisim { get; set; }
        public int RolID { get; set; }
        public string Mail { get; set; }
        public Rol Rol { get; set; }
    }
}
