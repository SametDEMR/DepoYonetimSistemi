using DepoYonetimSistemi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DepoYonetimSistemi.Controllers
{

    public class UrunController : Controller
    {


        private readonly ApplicationDbContext _context;

        public UrunController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UrunIslemleri()
        {
            var urundepolist = _context.urundepo.ToList();
            return View(urundepolist);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult UrunEkle()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        public IActionResult UrunDuzenle(int id)
        {
            var urundepolist = _context.urundepo.SingleOrDefault(k => k.ID == id);
            return View(urundepolist);
        }

        
        [HttpPost]
        public IActionResult UrunSil(int id)
        {
            if (id > 0)
            {
                _context.Database.ExecuteSqlInterpolated($"CALL AfterDeleteFromView({id});");
            }
            return RedirectToAction("UrunIslemleri"); // Listeleme sayfasına geri döner
        }

    }
}