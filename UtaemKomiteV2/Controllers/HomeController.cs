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
				if (file == null | file.Length < 1)
				{
					
					return Json("Hata: Dosya yok ya da Boş!");
				}
				Dosya dosya = new Dosya();
				dosya.tur = tur;
				dosya.tarih = DateTime.Now;
				dosya.isim = Path.GetFileNameWithoutExtension(file.FileName);
				dosya.boyut = Math.Round(Convert.ToDouble(file.Length),2);
				dosya.uzantı = Path.GetExtension(file.FileName);
				dosya.sysname = Arac.RandomString(8);
				dosya.kulName = HttpContext.Session.GetString("kulname");

				string pathPRE = Path.Combine(uploadsRoot, dosya.sysname + "PRE");
				FileStream yaratPRE = new FileStream(pathPRE, FileMode.CreateNew);
				file.CopyTo(yaratPRE);
				yaratPRE.Close();

				string path = Path.Combine(uploadsRoot, dosya.sysname);
				AES.EncryptFile(pathPRE, path);

				FileInfo fi = new FileInfo(pathPRE);
				fi.Delete();

				db.Add(dosya);
				db.SaveChanges();
				return Json("Index");
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
