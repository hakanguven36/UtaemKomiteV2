using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UtaemKomiteV2.Araclar;
using UtaemKomiteV2.Models;


namespace UtaemKomiteV2.Controllers
{
	[Yetki("superadmin")]
    public class SuperAdminController : Controller
    {
		private string uploadsRoot;
		MyContext db;
		IWebHostEnvironment hostEnvironment;
		public SuperAdminController(MyContext db, IWebHostEnvironment hostEnvironment)
		{
			this.db = db;
			this.hostEnvironment = hostEnvironment;
			uploadsRoot = hostEnvironment.WebRootPath + "/Dosyalar";
		}

        public IActionResult Index()
        {
            return View();
        }

		public IActionResult KullanıcıListele()
		{
			return PartialView(db.Kullar);
		}

		[HttpGet]
		public IActionResult KullanıcıYarat()
		{
			return PartialView();
		}
		[HttpPost]
		public IActionResult KullanıcıYarat(Kullar kul)
		{
			try
			{
				var mevcut = db.Kullar.Where(u => u.kulname == kul.kulname).FirstOrDefault();
				if(mevcut != null)
				{
					return Json("Bu kullanıcı adı zaten var!");
				}
				db.Kullar.Add(kul);
				db.SaveChanges();
				return Json("Tamam");
			}
			catch (Exception e)
			{
				return Json("Hata: " + e.Message);
			}

		}

		[HttpGet]
		public IActionResult KullanıcıDüzenle(int id)
		{
			return PartialView(db.Kullar.Find(id));
		}

		[HttpPost]
		public IActionResult KullanıcıDüzenle(Kullar kul)
		{
			try
			{
				db.Entry(kul).State = EntityState.Modified;
				db.SaveChanges();
				return Json("Tamam");
			}
			catch (Exception e)
			{
				return Json("Hata: " + e.Message);
			}
		}

		[HttpPost]
		public IActionResult KullanıcıSil(int id)
		{
			try
			{
				var kul = db.Kullar.Find(id);
				db.Kullar.Remove(kul);
				db.SaveChanges();
				return Json("Tamam");
			}
			catch (Exception e)
			{
				return Json("Hata: " + e.Message);
			}
		}

		[HttpPost]
		public IActionResult TümDosyalarıTemizle()
		{
			try
			{
				DirectoryInfo di = new DirectoryInfo(uploadsRoot);
				var allfiles = di.GetFiles();
				for (int i = 0; i < allfiles.Length; i++)
				{
					if(allfiles[i].Name != "yertutucu.txt")
					allfiles[i].Delete();
				}

				var dosyalar = db.Dosya.ToList();
				db.RemoveRange(dosyalar);
				db.SaveChanges();

				return Json("Tamam");
			}
			catch (Exception e)
			{
				return Json("Hata: " + e.Message);
			}

		}
	}
}