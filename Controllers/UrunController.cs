using DepoYonetimSistemi.Data;
using DepoYonetimSistemi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DepoYonetimSistemi.Controllers
{

    public class UrunController : Controller
    {


        private readonly ApplicationDbContext _context;

        public UrunController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UrunIslemleri()
        {
            var urundepolist = _context.urunler.Include(u => u.Depo).ToList();
            return View(urundepolist);
        }

        public IActionResult Depolar()
        {
            var depolist = _context.depolar.ToList();
            return View(depolist);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UrunEkle()
        {
            return View();
        }

        public IActionResult UrunEkleVeritaban(string UrunAd, int Fiyat, int StokDurumu, int depoid)
        {
            var urun = new Urun
            {
                Ad = UrunAd,
                Fiyat = Fiyat,
                StokDurumu = StokDurumu,
                DepoID = depoid
            };

            _context.urunler.Add(urun);
            _context.SaveChanges();
            return RedirectToAction("UrunIslemleri");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult UrunDuzenle(int id)
        {
            var urundepolist = _context.urunler.Include(u => u.Depo).SingleOrDefault(k => k.ID == id);
            return View(urundepolist);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UrunDuzenleVeritaban(int id, string ad, int fiyat, int StokDurumu)
        {
            // Veritabanında ürün güncelleme işlemi
            _context.Database.ExecuteSqlInterpolated($"UPDATE urunler SET Ad = {@ad}, Fiyat = {@fiyat}, StokDurumu = {@StokDurumu} WHERE ID = {@id}");
            return RedirectToAction("UrunIslemleri");
        }


        [HttpPost]
        public IActionResult UrunSil(int id)
        {
            if (id > 0)
            {
                _context.Database.ExecuteSqlInterpolated($"CALL AfterDeleteFromView({id});");
            }
            return RedirectToAction("UrunIslemleri"); // Listeleme sayfasına geri döner
        }


        public IActionResult DepoUrunListe(string DepoAdi)
        {
            var DepoIDAl = _context.depolar.SingleOrDefault(k => k.DepoAdi == DepoAdi);
            var depoUrunListesi = _context.urunler
                    .Include(u => u.Depo)
                    .Where(k => k.DepoID == DepoIDAl.DepoID) // DepoID değerine erişiyoruz
                    .ToList();
            return View(depoUrunListesi);
        }

    }
}