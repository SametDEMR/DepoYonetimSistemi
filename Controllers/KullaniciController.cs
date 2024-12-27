﻿using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin")]
        public ActionResult KullaniciEkleVeritaban(string ad, string soyad, string users, string mail)
        {
            _context.Database.ExecuteSqlInterpolated($"INSERT INTO kullanici(Isim, Soyisim, RolID, Mail) VALUES( {@ad}, {@soyad}, {@users}, {@mail})");

            return View("KullaniciIslemleri");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult KullaniciSec()
        {
            var kullaniciRollListesi = _context.KullaniciRoll.ToList();
            return View(kullaniciRollListesi);
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
