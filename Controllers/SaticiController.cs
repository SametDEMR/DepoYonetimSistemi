using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DepoYonetimSistemi.Controllers
{
    public class SaticiController : Controller
    {
        [Authorize(Roles = "Satici")]
        public IActionResult SatisSayfasi()
        {
            return View();
        }
    }
}
