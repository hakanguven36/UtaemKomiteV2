using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UtaemKomiteV2.Araclar;
using UtaemKomiteV2.Models;

namespace UtaemKomiteV2.Controllers
{
	[Yetki("user")]
	public class HomeController : Controller
	{
		string uploadsRoot;
		MyContext db;
		IWebHostEnvironment hostEnvironment;
		public HomeController(MyContext db, IWebHostEnvironment hostEnvironment)
		{
			this.db = db;
			this.hostEnvironment = hostEnvironment;
			uploadsRoot = hostEnvironment.WebRootPath + @"/Dosyalar";
		}

		public IActionResult Index()
		{
			return View(db.Dosya.ToList());
		}

		[HttpGet]
		public IActionResult YeniDosyaEkle(string dosyaTuru)
		{
			DOSYATURU tur = Enum.Parse<DOSYATURU>(dosyaTuru);
			return PartialView(new Dosya() { tur = tur });
		}

		[HttpPost]
		public IActionResult YeniDosyaEkle(string tur, IFormFile dosya)
		{
			
			try
			{
				DOSYATURU theTur;
				Enum.TryParse(tur, true, out theTur);

				if (dosya == null)
				{
					return Json("Hata: Dosya eklemediniz!");
				}
				else if (dosya.Length < 1)
				{
					return Json("Hata: Dosya boş görünüyor!");
				}
				Dosya d = new Dosya();
				d.tur = theTur;
				d.tarih = DateTime.Now;
				d.isim = Path.GetFileNameWithoutExtension(dosya.FileName);
				d.boyut = Math.Round(Convert.ToDouble(dosya.Length/1024),2);
				d.uzantı = Path.GetExtension(dosya.FileName);
				d.sysname = Arac.RandomString(8);
				d.kulName = HttpContext.Session.GetString("kulname");

				string path = Path.Combine(uploadsRoot, d.sysname);
				string hata = AES.EncryptFile(dosya, path);
				if(hata != "Tamam")
					return Json("Hata: " + hata);

				db.Add(d);
				db.SaveChanges();
				return Json("Tamam");
			}
			catch (Exception e)
			{
				return Json("Hata: " + e.Message);
			}
		}

		public IActionResult Dosyaİndir(int id)
		{
			return Json("Tamam");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
