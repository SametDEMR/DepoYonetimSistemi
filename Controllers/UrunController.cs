using DepoYonetimSistemi.Data;
using DepoYonetimSistemi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System.Xml.Linq;
using StackExchange.Redis;

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
        public ActionResult UrunIslemleri()
        {
            string url = "http://localhost:3000/soap?wsdl";
            var soapRequest = @"<?xml version=""1.0"" encoding=""utf-8""?>
                        <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
                                          xmlns:tns=""http://example.com/soap"">
                            <soapenv:Header/>
                            <soapenv:Body>
                                <tns:getData/>
                            </soapenv:Body>
                        </soapenv:Envelope>";

            using (var client = new System.Net.Http.HttpClient())
            {
                var content = new System.Net.Http.StringContent(soapRequest, System.Text.Encoding.UTF8, "text/xml");
                content.Headers.Add("SOAPAction", "getData");

                var response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;

                    // Yanıt XML formatında olduğu için önce parse ediyoruz
                    var xmlResponse = XDocument.Parse(responseString);

                    // "response" elementini güvenli bir şekilde al
                    var dataElement = xmlResponse.Descendants()
                                                 .FirstOrDefault(e => e.Name.LocalName == "response");

                    if (dataElement == null || string.IsNullOrEmpty(dataElement.Value))
                    {
                        throw new Exception("SOAP yanıtında 'response' elementi bulunamadı veya boş.");
                    }

                    // JSON formatında gelen veriyi deserialize et
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UrunDepo>>(dataElement.Value);

                    return View(data); // View'a gönder
                }
                else
                {
                    throw new Exception($"SOAP isteği başarısız oldu: {response.ReasonPhrase}");
                }
            }
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Depolar([FromServices] IConnectionMultiplexer redis)
        {
            // Redis bağlantısı üzerinden verileri alma
            var db = redis.GetDatabase();
            string cacheKey = "Depolar";

            // Redis'ten önbellekteki veriyi al
            string cachedDepolar = db.StringGet(cacheKey);

            List<Depo> depolist;
            if (!string.IsNullOrEmpty(cachedDepolar))
            {
                // Eğer önbellekte veri varsa deserialize et
                depolist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Depo>>(cachedDepolar);
            }
            else
            {
                // Ön bellekte yoksa veritabanından al
                depolist = _context.depolar.ToList();

                // Veriyi JSON formatında önbelleğe ekle (TTL: 5 dakika)
                db.StringSet(cacheKey, Newtonsoft.Json.JsonConvert.SerializeObject(depolist), TimeSpan.FromMinutes(5));
            }

            return View(depolist);
        }


    [Authorize(Roles = "Admin")]
        public IActionResult UrunEkle()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UrunEkleVeritaban(string UrunAd, int Fiyat, int StokDurumu, int depoid)
        {

            _context.Database.ExecuteSqlInterpolated($"CALL SPUrunEkle({UrunAd}, {Fiyat}, {StokDurumu}, {depoid});");
            return RedirectToAction("UrunIslemleri");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult UrunDuzenle(int id)
        {
            var urundepolist = _context.UrunDepo.SingleOrDefault(k => k.ID == id);
            return View(urundepolist);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UrunDuzenleVeritaban(int ID, string UrunAd, int Fiyat, int StokDurumu)
        {
            // Veritabanında ürün güncelleme işlemi
            _context.Database.ExecuteSqlInterpolated($"UPDATE urunler SET Ad = {@UrunAd}, Fiyat = {@Fiyat}, StokDurumu = {@StokDurumu} WHERE ID = {@ID}");
            return RedirectToAction("UrunIslemleri");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UrunSil(int id)
        {
            if (id > 0)
            {
                _context.Database.ExecuteSqlInterpolated($"delete from urunler where ID = {id}");
            }
            return RedirectToAction("UrunIslemleri"); // Listeleme sayfasına geri döner
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DepoUrunListe(string DepoAdi)
        {
            var depoUrunListesi = _context.UrunDepo.Where(k => k.DepoAdi == DepoAdi).ToList();
            return View(depoUrunListesi);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToplamUrunFiyat()
        {
            var veriler = await _context.ToplamFiyatMiktarAl
        .FromSqlInterpolated($"CALL ToplamFiyatMiktarAl()")
        .ToListAsync();
            return View(veriler);
        }

    }
}