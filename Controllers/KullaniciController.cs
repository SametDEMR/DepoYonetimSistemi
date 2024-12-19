using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DepoYonetimSistemi.Data;
using DepoYonetimSistemi.Models;

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


        public IActionResult KullaniciSec()
        {
            var kullaniciRollListesi = _context.kullaniciroll.ToList();

            // Verileri View'e gönderiyoruz
            return View(kullaniciRollListesi);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult KullaniciDuzenle(int id)
        {
            var kullanicilist = _context.kullaniciroll.SingleOrDefault(k => k.ID == id);
            return View(kullanicilist);
        }
    }
}
