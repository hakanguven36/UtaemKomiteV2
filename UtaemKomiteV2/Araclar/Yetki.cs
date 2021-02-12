using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtaemKomiteV2.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace UtaemKomiteV2.Araclar
{
	public class YetkiAttribute : ActionFilterAttribute
	{

		public string cont { get; set; }
		public YetkiAttribute(string cont) => this.cont = cont;

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			string kul = context.HttpContext.Session.GetString("kulrol");

			

			switch (cont)
			{
				case "superadmin":
					if (kul != "superadmin")
						Yetkisiz(context);
					break;
				case "admin":
					if (kul != "superadmin" & kul != "admin")
						Yetkisiz(context);
					break;
				case "user":
					if (kul != "superadmin" & kul != "admin" & kul != "user")
						Yetkisiz(context);
					break;
			}
		}

		private void Yetkisiz(ActionExecutingContext context)
		{
			context.Result =
				new RedirectToRouteResult(
				new RouteValueDictionary {
								{ "Controller", "Hesaplar" },
								{ "Action", "Index" }});
		}
	}

	public class KurulumYetkiAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (context.HttpContext.Session.GetString("kurulum") != "ok")
				context.Result = new RedirectToActionResult("Index", "Kurulum", null);

		}
	}
}
