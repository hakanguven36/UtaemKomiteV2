using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using UtaemKomiteV2.Models;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace UtaemKomiteV2.Araclar
{
	public static class Arac
	{
		#region BasicStringNumberOps
		public static string tooString(this object str)
		{
			return str == null ? "" : str.ToString();
		}

		public static string sapString(this String str, int baslangic, int uzunluk)
		{
			if (str.Length > uzunluk - baslangic)
			{
				return str.Substring(baslangic, uzunluk);
			}
			return str.Substring(baslangic, str.Length - baslangic);
		}

		private static Random random = new Random(DateTime.Now.Millisecond);
		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}
		public static string tooFirstUpper(this string str)
		{
			TextInfo ti = new CultureInfo("TR").TextInfo;
			return ti.ToTitleCase(str);
		}

		#endregion

		#region SymetricŞifreleme
		public static string encin(this string clearText)
		{
			return encin(clearText, null);
		}
		public static string encin(this string clearText, string hash)
		{
			if (clearText == null)
				return "";
			string EncryptionKey = hash == "default" || hash == "" || hash == null ? "Tesla" : hash;
			byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(clearBytes, 0, clearBytes.Length);
						cs.Close();
					}
					clearText = Convert.ToBase64String(ms.ToArray());
				}
			}
			return clearText;
		}
		public static string encout(this string clearText)
		{
			return encout(clearText, null);
		}
		public static string encout(this string cipherText, string hash)
		{
			if (cipherText == null)
				return "";
			string EncryptionKey = hash == "default" || hash == "" || hash == null ? "Tesla" : hash;

			cipherText = cipherText.Replace(" ", "+");
			byte[] cipherBytes = Convert.FromBase64String(cipherText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cs.Write(cipherBytes, 0, cipherBytes.Length);
						cs.Close();
					}
					cipherText = Encoding.Unicode.GetString(ms.ToArray());
				}
			}
			return cipherText;
		}
		#endregion
		#region DosyaŞifreleme

		#endregion
		#region Email

		// ADD THIS TO CONFIG <system.net></system.net>
		// <mailSettings>
		//  <smtp from = "you@outlook.com" >
		//  < network host="smtp-mail.outlook.com" 
		//             port="587" 
		//             userName="you@outlook.com"
		//             password="password" 
		//             enableSsl="true" />
		//  </smtp>
		// </mailSettings>
		// AND EPOSTA MODEL TO DATABASE

		/// <returns>Hata var ise ErrorString x Yoksa null döndürür</returns>
		//public static string SendEmail(Eposta eposta)
		//{
		//	try
		//	{
		//		var message = new MailMessage();
		//		message.To.Add(new MailAddress(eposta.receiver)); // can add more than one
		//		// message.Bcc.Add(new MailAddress(eposta.receiver)); // this makes users prevent to see each other
		//		message.Subject = eposta.subject;
		//		message.Body = eposta.body;
		//		message.IsBodyHtml = eposta.isHtml;
		//		message.Sender = new MailAddress(eposta.sender);

		//		using (var smtp = new SmtpClient())
		//		{
		//			smtp.SendMailAsync(message);
		//			return null;
		//		}
		//	}
		//	catch (Exception e)
		//	{
		//		return "Error: " + e.Message;
		//	}
		//}
		#endregion
	}

	public static class AES
	{
		public static string EncryptFile(IFormFile dosya, string outputFile)
		{
			try
			{
				UnicodeEncoding UE = new UnicodeEncoding();
				byte[] key = UE.GetBytes("MyKEY001");
				RijndaelManaged RMCrypto = new RijndaelManaged();

				Stream stream = dosya.OpenReadStream();
				MemoryStream ms = new MemoryStream();
				stream.CopyTo(ms);
				byte[] buffer = ms.ToArray();

				FileStream outputFS = new FileStream(outputFile, FileMode.Create);
				CryptoStream cs = new CryptoStream(outputFS, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write);
				cs.Write(buffer, 0, buffer.Length);
				cs.Close();
				outputFS.Close();
				return "Tamam";
			}
			catch(Exception e)
			{
				return e.Message;
			}
		}

		public static void DecryptFile(string inputFile, string outputFile)
		{
			try
			{
				UnicodeEncoding UE = new UnicodeEncoding();
				byte[] key = UE.GetBytes("MyKEY001");
				FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
				RijndaelManaged RMCrypto = new RijndaelManaged();
				CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read);
				FileStream fsOut = new FileStream(outputFile, FileMode.Create);
				int data;
				while ((data = cs.ReadByte()) != -1)
					fsOut.WriteByte((byte)data);
				fsOut.Close();
				cs.Close();
				fsCrypt.Close();
			}
			catch
			{

			}
		}
	}

	public static class ControllerExtensions
	{
		public static async Task<string> RenderViewAsync<TModel>(this Controller controller, string viewName, TModel model, bool partial = false)
		{
			if (string.IsNullOrEmpty(viewName))
			{
				viewName = controller.ControllerContext.ActionDescriptor.ActionName;
			}

			controller.ViewData.Model = model;

			using (var writer = new StringWriter())
			{
				IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
				ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, !partial);

				if (viewResult.Success == false)
				{
					return $"A view with the name {viewName} could not be found";
				}

				ViewContext viewContext = new ViewContext(
					controller.ControllerContext,
					viewResult.View,
					controller.ViewData,
					controller.TempData,
					writer,
					new HtmlHelperOptions()
				);

				await viewResult.View.RenderAsync(viewContext);

				return writer.GetStringBuilder().ToString();
			}
		}
	}

	public class Pagination<T>
	{
		public int pageSize { get; set; }
		public int itemCount { get; set; }
		public int totalPage { get; set; }
		public int currentPage { get; set; }
		public bool isFirstPage { get; set; }
		public bool isLastPage { get; set; }
		public Pagination(int pageSize) => this.pageSize = pageSize;

		public List<T> GetPage(List<T> liste, int pageNo)
		{
			itemCount = liste.Count();
			totalPage = itemCount / pageSize;
			if (pageNo >= totalPage)
			{
				pageNo = totalPage;
				isLastPage = true;
			}
			if (pageNo <= 1)
			{
				pageNo = 1;
				isFirstPage = true;
			}
			currentPage = pageNo;
			int skip = (pageNo - 1) * pageSize;
			return liste.Skip(skip).Take(pageSize).ToList();
		}
	}
}
