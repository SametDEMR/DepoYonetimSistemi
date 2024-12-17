using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DepoYonetimSistemi.Controllers
{
    public class UrunController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult UrunIslemleri()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult UrunEkle()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult UrunDuzenle()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult UrunSil()
        {
            return View();
        }
    }
}