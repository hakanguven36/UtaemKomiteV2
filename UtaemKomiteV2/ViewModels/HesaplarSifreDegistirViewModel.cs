using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UtaemKomiteV2.ViewModels
{
	public class HesaplarSifreDegistirViewModel
	{
		public int ID { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Mevcut Şifreniz")]
		public string oldpassword { get; set; }

		[Required]
		[StringLength(12, MinimumLength = 4, ErrorMessage = "4-12 karakter olmalı!")]
		[DataType(DataType.Password)]
		[Display(Name = "Yeni Şifreniz")]
		public string newpassword { get; set; }

		[Compare("newpassword",ErrorMessage ="Şifreler tutarsız!")]
		[DataType(DataType.Password)]
		[Display(Name = "Yeni Şifreniz(tekrar)")]
		public string newpassword2 { get; set; }
	}
}
