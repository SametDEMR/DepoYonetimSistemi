namespace DepoYonetimSistemi.Models
{
    public class SearchUsers
    {
        public int ID { get; set; }
        public string Isim { get; set; }
        public string Soyisim { get; set; }
        public int RolID { get; set; } // Ensure this matches the stored procedure's output
        public string Mail { get; set; }
        public int DepoID { get; set; } // Ensure this matches the stored procedure's output
    }
}
