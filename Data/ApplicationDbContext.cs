using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using DepoYonetimSistemi.Models;

namespace DepoYonetimSistemi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<KullaniciRoll> kullaniciroll { get; set; }

        public DbSet<UrunDepo> urundepo { get; set; }

    }
}
