using DepoYonetimSistemi.Data;
using DepoYonetimSistemi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DepoYonetimSistemi.Controllers
{
    public class SaticiController : BaseController
    {

        private readonly ApplicationDbContext _context;


        public SaticiController(ApplicationDbContext context)
        {
            _context = context;
        }


        [Authorize(Roles = "Satici")]
        public IActionResult SatisSayfasi()
        {
            var urundepolist = _context.UrunDepo.ToList();
            return View(urundepolist);
        }


        [Authorize(Roles = "Satici")]
        public IActionResult SatisIslemi(int id)
        {
            var urundepolist = _context.UrunDepo.SingleOrDefault(k => k.ID == id);
            return View(urundepolist);
        }


        [HttpPost]
        public IActionResult UrunSatVeritaban(int UrunID, int SatisMiktari, string UrunAd)
        {
            if (SatisMiktari >= 0)
            {
                var urun = _context.urunler.FirstOrDefault(u => u.ID == UrunID);

                urun.Ad = UrunAd; // Sadece Ad alanını güncelle
                urun.StokDurumu = urun.StokDurumu - SatisMiktari; // StokDurumu'nu güncelle

                _context.urunler.Update(urun);
                _context.SaveChanges();

                int KullaniciID = GetLoggedInUserId().GetValueOrDefault();

                var satis = new Satislar
                {
                    KullaniciID = @KullaniciID,
                    UrunAdi = @UrunAd,
                    Miktar = @SatisMiktari
                };
                _context.satislar.Add(satis);
                _context.SaveChanges();


                return RedirectToAction("SatisSayfasi");
            }
            else
            {
                return RedirectToAction("SatisIslemi", new { id = UrunID });
            }

        }


        public IActionResult SatislarimSayfasi()
        {
            int KullaniciID = GetLoggedInUserId().GetValueOrDefault();
            var satislist = _context.satislar.Where(u => u.KullaniciID == KullaniciID).ToList();
            return View(satislist);
        }


        public IActionResult UrunTalebiSayfasi()
        {
            int DepoID = GetLoggedInDepoId().GetValueOrDefault();
            var depolist = _context.UrunDepo.Where(u => u.DepoID == DepoID).ToList();
            return View(depolist);
        }

        public IActionResult SaticiUrunTalep(string UrunAd, int Miktar)
        {
            int DepoID = GetLoggedInDepoId().GetValueOrDefault();

            _context.Database.ExecuteSqlInterpolated($"INSERT INTO talepler (UrunAdi, Miktar, DepoID) VALUES ({UrunAd}, {Miktar}, {DepoID})");            

            return RedirectToAction("UrunTaleplerimSayfasi");

        }

        
        public IActionResult UrunTaleplerimSayfasi()
        {
            int DepoID = GetLoggedInDepoId().GetValueOrDefault();
            var taleplist = _context.TalepDepo.Where(u => u.DepoID == DepoID).ToList();
            return View(taleplist);
        }


        public IActionResult UrunTalepSil(int TalepID)
        {
            _context.Database.ExecuteSqlInterpolated($"DELETE FROM talepler WHERE TalepID = {TalepID}");
            return RedirectToAction("UrunTaleplerimSayfasi");
        }
    }
}
