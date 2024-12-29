using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DepoYonetimSistemi.Data;

namespace DepoYonetimSistemi.Controllers
{
    public class GenelController : BaseController
    {

        public IActionResult HaritaSayfasi(string yer)
        {
            @ViewBag.City = yer;
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
            var user = _context.KullaniciRollDepo.FirstOrDefault(k => k.Isim == username); // Kullanıcının giriş bilgilerini kontrol ediyoruz

            if (user == null)
            {
                ViewBag.ErrorMessage = "Kullanıcı bulunamadı.";
                return View();
            }

            // Kullanıcının rolünü al
            string KullaniciRoll = user.RolAdi ?? "Unknown";

            HttpContext.Session.SetInt32("UserID", user.ID);
            HttpContext.Session.SetInt32("DepoId", user.DepoID);

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

        public IActionResult YetkisizErisim()
        {
            return View();
        }

        public IActionResult KarsilamaEkrani()
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
