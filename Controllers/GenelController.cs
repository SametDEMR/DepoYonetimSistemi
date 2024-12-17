using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DepoYönetimSistemi.Controllers
{
    public class GenelController : Controller
    {
        public async Task<IActionResult> GirisSayfasiAsync(string username)
        {
            string Kullaniciroll = "";

            if (username == "1")
                Kullaniciroll = "Admin";
            else if (username == "2")
                Kullaniciroll = "Satici";
            else
                return View(); // Yanlış kullanıcı girişi

            // Claims oluştur
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, Kullaniciroll) // Rol burada ekleniyor
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
            return View();
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
