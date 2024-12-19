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
        private readonly ApplicationDbContext _context;

        public GenelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GirisSayfasiAsync(string username)
        {
            // Veritabanından kullanıcıyı kontrol et
            var user = await _context.kullaniciroll
                .FirstOrDefaultAsync(u => u.Isim == username);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Kullanıcı bulunamadı.";
                return View();
            }

            var model = new KullaniciRoll
            {
                ID = user.ID,
                Isim = user.Isim,
                Soyisim = user.Soyisim,
                Mail = user.Mail,
                RolAdi = user.RolAdi,
            };

            // Kullanıcının rolünü al
            string Kullaniciroll = user.RolAdi ?? "Unknown";

            // Claims oluştur
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Isim),
                new Claim(ClaimTypes.Role, Kullaniciroll)
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
                var kullanicilist = _context.kullaniciroll.FirstOrDefault(k => k.ID == userIdInt);
                return View(kullanicilist); // Kullanıcı bilgilerini View'e gönder
            }
            else
            {
                var kullanicilist = _context.kullaniciroll.FirstOrDefault(k => k.ID == userIdInt);
                return View(kullanicilist); // Kullanıcı bilgilerini View'e gönder
                return View(kullanicilist);
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
