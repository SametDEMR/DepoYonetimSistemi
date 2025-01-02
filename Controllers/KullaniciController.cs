using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DepoYonetimSistemi.Data;
using DepoYonetimSistemi.Models;
using System.Text.Json;
using Grpc.Net.Client;
using Grpc.Core;

namespace DepoYonetimSistemi.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KullaniciController(ApplicationDbContext context)
        {
            _context = context;
        }


        [Authorize(Roles = "Admin")]
        public ActionResult KullaniciIslemleri()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult KullaniciEkle()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult KullaniciEkleVeritaban(string ad, string soyad, string users, string mail, int DepoID)
        {
            _context.Database.ExecuteSqlInterpolated($"CALL SPKullaniciEkle( {@ad}, {@soyad}, {@users}, {@mail}, {DepoID})");

            return View("KullaniciIslemleri");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult KullaniciSec(int id)
        {
            if (id == null)
            {
                return View(id);
            }
            else
            {
                using var channel = GrpcChannel.ForAddress("http://localhost:50051"); // Servis adresini buraya yazın

                // gRPC istemcisi oluştur
                var client = new UrunService.UrunServiceClient(channel);

                // İstek gönder
                var request = new GetDataRequest { Args = id.ToString() ?? string.Empty };
                var response = client.GetData(request);

                var kullanicilar = JsonSerializer.Deserialize<IEnumerable<KullaniciRoll>>(response.Response);

                // Modeli View'e gönder
                return View(kullanicilar);
            }
        }


        [Authorize(Roles = "Admin")]
        public ActionResult FiltreliKullanici(int? ID, string Ad, string Soyad, string Rol, string Mail)
        {
            var kullanicilar = _context.KullaniciRoll
                .Where(k =>
                    (!ID.HasValue || k.ID == ID.Value) &&
                    (string.IsNullOrEmpty(Ad) || k.Isim.Contains(Ad)) &&
                    (string.IsNullOrEmpty(Soyad) || k.Soyisim.Contains(Soyad)) &&
                    (string.IsNullOrEmpty(Rol) || k.RolAdi.Contains(Rol)) &&
                    (string.IsNullOrEmpty(Mail) || k.Mail.Contains(Mail))
                )
                .ToList();

            // Sonuçları view'a gönderiyoruz
            return View(kullanicilar);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult KullaniciDuzenle(int id)
        {
            var kullanicilist = _context.KullaniciRoll.SingleOrDefault(k => k.ID == id);
            return View(kullanicilist);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult KullaniciDuzenleVeritaban(int id, string ad, string soyad, string eposta, string rolad)
        {
            _context.Database.ExecuteSqlInterpolated($"UPDATE kullanici SET Isim = {@ad}, Soyisim = {@soyad}, RolID = {@rolad}, Mail = {@eposta} WHERE ID = {@id}");
            return RedirectToAction("KullaniciIslemleri");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult KullaniciSil(int id)
        {
            if (id > 0)
            {
                _context.Database.ExecuteSqlInterpolated($"DELETE FROM kullanici WHERE id = {id}");
            }
            return RedirectToAction("KullaniciIslemleri"); // Listeleme sayfasına geri döner
        }
    }
}
