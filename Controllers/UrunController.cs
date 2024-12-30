using DepoYonetimSistemi.Data;
using DepoYonetimSistemi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;

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
        public ActionResult UrunIslemleri()
        {
            var data = _context.UrunDepo.ToList();
            return View(data);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public IActionResult UrunEkleVeritaban(string UrunAd, int Fiyat, int StokDurumu, int depoid)
        {

            _context.Database.ExecuteSqlInterpolated($"CALL SPUrunEkle({UrunAd}, {Fiyat}, {StokDurumu}, {depoid});");
            return RedirectToAction("UrunIslemleri");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult UrunDuzenle(int id)
        {
            var urundepolist = _context.UrunDepo.SingleOrDefault(k => k.ID == id);
            return View(urundepolist);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UrunDuzenleVeritaban(int ID, string UrunAd, int Fiyat, int StokDurumu)
        {
            // Veritabanında ürün güncelleme işlemi
            _context.Database.ExecuteSqlInterpolated($"UPDATE urunler SET Ad = {@UrunAd}, Fiyat = {@Fiyat}, StokDurumu = {@StokDurumu} WHERE ID = {@ID}");
            return RedirectToAction("UrunIslemleri");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UrunSil(int id)
        {
            if (id > 0)
            {
                _context.Database.ExecuteSqlInterpolated($"delete from urunler where ID = {id}");
            }
            return RedirectToAction("UrunIslemleri"); // Listeleme sayfasına geri döner
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DepoUrunListe(string DepoAdi)
        {
            var depoUrunListesi = _context.UrunDepo.Where(k => k.DepoAdi == DepoAdi).ToList();
            return View(depoUrunListesi);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToplamUrunFiyat()
        {
            var veriler = await _context.ToplamFiyatMiktarAl
        .FromSqlInterpolated($"CALL ToplamFiyatMiktarAl()")
        .ToListAsync();
            return View(veriler);
        }

    }
}