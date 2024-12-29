using Microsoft.AspNetCore.Mvc;
using DepoYonetimSistemi.Models; // Model sınıfının bulunduğu namespace

namespace DepoYonetimSistemi.ViewComponents
{
    public class UrunListesiViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<UrunDepo> urunler)
        {
            return View(urunler);
        }
    }
}
