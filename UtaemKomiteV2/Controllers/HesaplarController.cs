using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UtaemKomiteV2.Araclar;
using UtaemKomiteV2.Models;
using UtaemKomiteV2.ViewModels;

namespace UtaemKomiteV2.Controllers
{
	public class HesaplarController : Controller
	{
		MyContext db;
		IConfiguration configuration;
		public HesaplarController(MyContext db, IConfiguration configuration)
		{
			this.db = db;
			this.configuration = configuration;
		}

		public IActionResult Index()
		{
			if (ÇerezOku())
				return RedirectToAction("Index", "Home");
			return View();
		}

		[HttpPost]
		public IActionResult Index(HesaplarViewModel model)
		{
			
			if (ModelState.IsValid)
			{
				string kulnameNormalized = model.username.ToUpper();
				Kullar kul = db.Kullar.Where(u => u.kulname == kulnameNormalized).FirstOrDefault();
				if (kul == null)
				{
					ModelState.AddModelError("", "Kullanıcı adı ve şifrenizi kontrol ediniz");
				}
				else
				{
					if (kul.kulpass != model.password)
					{
						HataArttır(kul);
						ModelState.AddModelError("", "Kullanıcı adı ve şifrenizi kontrol ediniz");
					}
					else
					{
						YetkiVer(kul);
						if (model.remember)
							ÇerezYap(kul);
						else
							ÇerezSil(kul);
						return RedirectToAction("Index", "Home");
					}
				}
			}
			return View(model);
		}

		private void YetkiVer(Kullar kul)
		{
			if (kul.kulname == "oguz")
			{
				SuperYetkiVer(kul);
				return;
			}

			HttpContext.Session.SetInt32("kulID", kul.ID);
			HttpContext.Session.SetString("kulname", kul.kulname);
			if (kul.admin)
				HttpContext.Session.SetString("kulrol", "admin");
			else
				HttpContext.Session.SetString("kulrol", "user");
		}

		private void SuperYetkiVer(Kullar kul)
		{
			//kmly = "byRZPjKnntjAk5aOp2FkrQ=="
			HttpContext.Session.SetInt32("kulID", kul.ID);
			HttpContext.Session.SetString("kulname", kul.kulname);
			HttpContext.Session.SetString("kulrol", "superadmin");
		}

		private bool ÇerezYap(Kullar kul)
		{
			try
			{
				string çerez = Arac.RandomString(28);
				CookieOptions options = new CookieOptions();
				options.Expires = DateTime.Now.AddDays(5);
				Response.Cookies.Append("fastlogin", çerez);

				kul.hatirla = true;
				kul.cerez = çerez;
				db.Entry(kul).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
				db.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}

		private bool ÇerezOku()
		{
			string çerez = Request.Cookies["fastlogin"].encin();
			if (string.IsNullOrWhiteSpace(çerez))
				return false;
			Kullar kul = db.Kullar.FirstOrDefault(u => u.cerez == çerez & u.hatirla == true);
			if (kul != null)
			{
				YetkiVer(kul);
				return true;
			}
			return false;
		}

		private void ÇerezSil(Kullar kul)
		{
			kul.hatirla = false;
			kul.cerez = "";
			db.Entry(kul).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			db.SaveChanges();
			Response.Cookies.Delete("fastlogin");
		}

		private bool HataArttır(Kullar kul)
		{
			bool ret = true;
			kul.hatali++;
			if (kul.hatali > 5)
			{
				kul.kilitliTarih = DateTime.Now.AddMinutes(15);
				ret = false;
			}
			db.Entry(kul).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			db.SaveChanges();
			return ret;
		}

		[Yetki("user")]
		public IActionResult ŞifreDeğiştir()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Yetki("user")]
		public IActionResult ŞifreDeğiştir(HesaplarSifreDegistirViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);
			int kulID = HttpContext.Session.GetInt32("kulID") ?? 0;
			Kullar kul = db.Kullar.FirstOrDefault(u => u.ID == kulID);

			if (kul.kulpass != model.oldpassword)
			{
				ModelState.AddModelError("", "Şifre hatalı!");
				return View(model);
			}
			else
			{
				kul.kulpass = model.newpassword;
				db.Entry(kul).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
				db.SaveChanges();
				ViewBag.cevap = "Tamam";
			}

			return View();
		}

		[Yetki("user")]
		public IActionResult ÇıkışYap()
		{
			Kullar kul = db.Kullar.Find(HttpContext.Session.GetInt32("kulID") ?? 0);
			if (kul != null)
				ÇerezSil(kul);

			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home");
		}

		public IActionResult KurulumYap()
		{
			try
			{
				if(db.Kullar.FirstOrDefault(u=>u.kulname =="oguz") == null)
				{
					string user = configuration.GetSection("Kurulum").GetSection("superuser").Value;
					string pass = configuration.GetSection("Kurulum").GetSection("superpass").Value;
					Kullar kul = new Kullar();
					kul.kulname = user;
					kul.kulpass = pass;
					db.Kullar.Add(kul);
					db.SaveChanges();
					return Json("Tamam");
				}
				return Json("Kurucu üye zaten var!");
			}
			catch (Exception e)
			{
				return PartialView(e.Message);
			}
		}
	}
}