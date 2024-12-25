using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using DepoYonetimSistemi.Data;
using DepoYonetimSistemi.Models;

namespace DepoYonetimSistemi.Controllers
{
    public class GenelController : Controller
    {
        public IActionResult Misafir()
        {
            return View();
        }


        private readonly ApplicationDbContext _context;

        public GenelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GirisSayfasiAsync(string username)
        {
            // Veritabanından kullanıcıyı kontrol et
            var user = _context.kullanici
                .Include(k => k.Rol) // Rol bilgisini dahil ediyoruz
                .FirstOrDefault(k => k.Isim == username); // Kullanıcının giriş bilgilerini kontrol ediyoruz

            if (user == null)
            {
                ViewBag.ErrorMessage = "Kullanıcı bulunamadı.";
                return View();
            }

            // Kullanıcının rolünü al
            string KullaniciRoll = user.Rol.RolAdi ?? "Unknown";

            // Claims oluştur
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Isim),
                new Claim(ClaimTypes.Surname, user.Soyisim),
                new Claim(ClaimTypes.Email, user.Mail),
                new Claim(ClaimTypes.Role, KullaniciRoll) // Kullanıcının rolü
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Kullanıcıyı oturum açtır
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return RedirectToAction("AnaMenü");

        }


        public IActionResult AnaMenü()
        {
            return View();
        }

        public IActionResult Profil()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcının ID'si

            if (int.TryParse(userId, out int userIdInt))
            {
                var kullanicilist = _context.kullanici.FirstOrDefault(k => k.ID == userIdInt);
                return View(kullanicilist); // Kullanıcı bilgilerini View'e gönder
            }
            else
            {
                var kullanicilist = _context.kullanici.FirstOrDefault(k => k.ID == userIdInt);
                return View(kullanicilist); // Kullanıcı bilgilerini View'e gönder
            }
        }

        public IActionResult YetkisizErisim()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cikis()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("GirisSayfasi", "Genel");
        }
    }
}
