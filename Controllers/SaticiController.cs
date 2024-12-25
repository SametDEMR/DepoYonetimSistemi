using DepoYonetimSistemi.Data;
using DepoYonetimSistemi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DepoYonetimSistemi.Controllers
{
    public class SaticiController : Controller
    {

        private readonly ApplicationDbContext _context;

        public SaticiController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Satici")]
        public IActionResult SatisSayfasi()
        {
            var urundepolist = _context.urunler.Include(u => u.Depo).ToList();
            return View(urundepolist);
        }

        [Authorize(Roles = "Satici")]
        public IActionResult SatisIslemi(int id)
        {
            var urundepolist = _context.urunler.SingleOrDefault(k => k.ID == id);
            return View(urundepolist);
        }

        [Authorize(Roles = "Satici")]
        public IActionResult UrunSatVeritaban(int ID, int SatisMiktar, string UrunAd)
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            // Veritabanında ürün güncelleme işlemi
            _context.Database.ExecuteSqlInterpolated($"update urunler set StokDurumu = StokDurumu - {@SatisMiktar} Where ID = {@ID}");
            //_context.Database.ExecuteSqlInterpolated($"INSERT INTO Satislar (KullaniciID, UrunAdi, Miktar) VALUES ({@userId}, {@UrunAd}, {@SatisMiktar})");
            return RedirectToAction("SatislarimSayfasi");
        }

        public IActionResult SatislarimSayfasi()
        {
            var satislist = _context.satislar.ToList();
            return View(satislist);
        }
        public IActionResult UrunTalebiSayfasi()
        {
            return View();
        }

        public IActionResult SaticiUrunTalep(string UrunAd, int Miktar)
        {
            _context.Database.ExecuteSqlInterpolated($"INSERT INTO talepler (UrunAdi, Miktar, DepoID) VALUES ({UrunAd}, {Miktar}, 1)");
            return RedirectToAction("UrunTaleplerimSayfasi");

        }
        public IActionResult UrunTaleplerimSayfasi()
        {
            var taleplist = _context.talepler.Include(u => u.Depo).ToList();
            return View(taleplist);
        }

        public IActionResult UrunTalepSil(int TalepID)
        {
            _context.Database.ExecuteSqlInterpolated($"DELETE FROM talepler WHERE TalepID = {TalepID}");
            return RedirectToAction("UrunTaleplerimSayfasi");
        }
    }
}
