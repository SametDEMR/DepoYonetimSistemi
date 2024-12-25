using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using DepoYonetimSistemi.Models;

namespace DepoYonetimSistemi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kullanici>()
                .HasOne(k => k.Rol) // Kullanıcı bir Role sahiptir
                .WithMany(r => r.Kullanicilar) // Bir Rol birçok kullanıcıya sahiptir
                .HasForeignKey(k => k.RolID); // Kullanıcıdaki yabancı anahtar RolID
        }

        public DbSet<Kullanici> kullanici { get; set; }

        public DbSet<Depo> depolar { get; set; }

        public DbSet<Rol> roller { get; set; }

        public DbSet<Urun> urunler { get; set; }

        public DbSet<Talep> talepler { get; set; }

        public DbSet<Satislar> satislar { get; set; }
    }
}
