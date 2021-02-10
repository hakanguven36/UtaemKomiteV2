using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
		private string uploadsRoot;
		MyContext db;
		IWebHostEnvironment hostEnvironment;
		public HomeController(MyContext db, IWebHostEnvironment hostEnvironment)
		{
			this.db = db;
			this.hostEnvironment = hostEnvironment;
			uploadsRoot = hostEnvironment.WebRootPath + "/Dosyalar";
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
		public IActionResult YeniDosyaEkle(DOSYATURU tur, IFormFile file)
		{
			try
			{
				Dosya dosya = new Dosya();
				dosya.tur = tur;
				dosya.tarih = DateTime.Now;
				dosya.isim = Path.GetFileNameWithoutExtension(file.FileName);
				dosya.boyut = Math.Round(Convert.ToDouble(file.Length),2);
				dosya.uzantı = Path.GetExtension(file.FileName);
				dosya.sysname = Arac.RandomString(8);

				string path = Path.Combine(uploadsRoot, dosya.sysname);
				using(var filestream = new FileStream(path, FileMode.CreateNew))
				{
					Stream a = file.OpenReadStream();
					
					file.CopyTo(filestream);
				}
				db.Add(dosya);
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				ViewBag.Hata = e.Message;
				return View();
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
