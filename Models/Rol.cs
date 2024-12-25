namespace DepoYonetimSistemi.Models
{
    public class Rol
    {
        public int ID { get; set; }
        public string RolAdi { get; set; }
        public ICollection<Kullanici> Kullanicilar { get; set; }
    }
}
