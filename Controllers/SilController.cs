using DepoYonetimSistemi.Data;
using DepoYonetimSistemi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DepoYonetimSistemi.Controllers
{
    public class SilController : Controller
    {

        private readonly ApplicationDbContext _context;


        public SilController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult TabloSil()
        {
            return View();
        }

        public ActionResult İliskiliSil()
        {
            // Controller'da
            var result = _context.Mesaj
                .FromSqlInterpolated($"CALL KontrolVeSilProsedur('depolar');")
                .AsEnumerable()
                .FirstOrDefault();

            TempData["ResultMessage"] = result.IslemSonucu;

            return RedirectToAction("TabloSil");
        }



        public ActionResult İliskisizSil()
        {
            // Controller'da
            var result = _context.Mesaj
                .FromSqlInterpolated($"CALL KontrolVeSilProsedur('deneme');")
                .AsEnumerable()
                .FirstOrDefault();

            TempData["ResultMessage"] = result.IslemSonucu;

            return RedirectToAction("TabloSil");
        }
    }
}
