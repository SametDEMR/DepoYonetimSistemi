using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DepoYonetimSistemi.Controllers
{
    public class KullaniciController : Controller
    {
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
        public ActionResult KullaniciSec()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult KullaniciDuzenle()
        {
            return View();
        }
    }
}
