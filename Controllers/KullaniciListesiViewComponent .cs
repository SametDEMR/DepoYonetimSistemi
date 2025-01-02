using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DepoYonetimSistemi.Models;

namespace DepoYonetimSistemi.ViewComponents
{
    public class KullaniciListesiViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<KullaniciRoll> kullanicilar)
        {
            return View(kullanicilar);
        }
    }
}
