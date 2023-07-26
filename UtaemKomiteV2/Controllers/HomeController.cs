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
using MimeMapping;
using Microsoft.EntityFrameworkCore;

namespace UtaemKomiteV2.Controllers
{
	[Yetki("user")]
	public class HomeController : Controller
	{
		MyContext db;
		string uploadsRoot;
		
		public HomeController(MyContext db, IWebHostEnvironment hostEnvironment)
		{
			this.db = db;
			uploadsRoot = hostEnvironment.WebRootPath + @"/Dosyalar";
		}

		public IActionResult Index()
		{
			var dosyaList = db.Dosya.Include(u=>u.tur).Where(u => u.silindi != true).OrderByDescending(u=>u.tarih).ToList();
			ViewBag.turList = db.Tur.Select(u => u.isim).ToList();
			return View(dosyaList);
		}

		[HttpGet]
		public IActionResult YeniDosyaEkle(string dosyaTuru)
		{
			ViewBag.dosyaTuru = dosyaTuru;
			return PartialView();
		}

		[HttpPost]
		public IActionResult YeniDosyaEkle(string dosyaTuru, IFormFile dosya)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(dosyaTuru))
					throw new Exception("Hata: Dosya türü belirsiz!");
				Tur theTur = db.Tur.FirstOrDefault(u=>u.isim == dosyaTuru);
				if (theTur == null)
					throw new Exception("Hata: tür seçimi hatalı!");
				if (dosya == null)
					throw new Exception("Hata: Dosya eklemediniz!");
				if (dosya.Length < 1)
					throw new Exception("Hata: Dosya boş görünüyor!");

				string sysname = Arac.RandomString(16);
				string path = Path.Combine(uploadsRoot, sysname);

				using (Stream gelenStream = dosya.OpenReadStream())
				using (MemoryStream ms = new MemoryStream())
				{
					gelenStream.CopyTo(ms);
					using (MemoryStream output = new SIFRELEME().Kilitle(ms))
					using (FileStream fs = new FileStream(path, FileMode.Create))
						fs.Write(output.ToArray(), 0, Convert.ToInt32(output.Length));
				}


				Dosya d = new Dosya();
				d.tur = theTur;
				d.tarih = DateTime.Now;
				d.isim = Path.GetFileNameWithoutExtension(dosya.FileName);
				d.boyut = dosya.Length;
				d.uzantı = Path.GetExtension(dosya.FileName);
				d.sysname = sysname;
				d.kulName = HttpContext.Session.GetString("kulname");
				IconMaker icon = new IconMaker();
				d.icon = icon.Yap(dosya.FileName);
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
			MemoryStream output = new MemoryStream();
			
			try
			{
				var d = db.Dosya.FirstOrDefault(u=>u.ID == id);
				if(d==null) throw new Exception("Hata: Dosya ID'si hatalı");
				string path = Path.Combine(uploadsRoot, d.sysname);

				using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
				using (MemoryStream ms = new MemoryStream())
				{
					fs.CopyTo(ms);
					output = new SIFRELEME().KilitAç(ms);
					output.Capacity = Convert.ToInt32(d.boyut);
				}

				string mimeType = MimeUtility.GetMimeMapping(d.isim + d.uzantı);
				string downloadName = d.isim + d.uzantı;

				return File(output.GetBuffer(), mimeType, downloadName);
			}
			catch (Exception e)
			{
				return Json("Hata: " + e.Message);
			}
			finally
			{
				HttpContext.Response.OnCompleted(async () => await Task.Run(() => output.Dispose()));
			}
			
		}

		public IActionResult DosyaSil(int id)
		{
			try
			{
				var dosya = db.Dosya.FirstOrDefault(u => u.ID == id);
				if (dosya == null)
					return Json("Hata: Böyle bir id yok!");
				dosya.silindi = true;
				db.Entry(dosya).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
				db.SaveChanges();
				return Json("Tamam");
			}
			catch (Exception e)
			{
				return Json(e.Message);
			}
		}

		
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}

	public class IconMaker{

		Dictionary<string, string> dic = new Dictionary<string, string>()
		{
			{".pdf","pdflogo.png" },
			{".doc","wordlogo.png" },
			{".docx","wordlogo.png" },
			{".xls","excellogo.png" },
			{".xlsx","excellogo.png" },
			{".bmp","picturelogo.png" },
			{".png","picturelogo.png" },
			{".jpg","picturelogo.png" },
			{".jpeg","picturelogo.png" }
		};
		public string Yap(string filename) => dic.ContainsKey(Path.GetExtension(filename)) ?
			dic[Path.GetExtension(filename)] :
			"unknownlogo.png";
	}
}
