using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DepoYönetimSistemi.Controllers
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
