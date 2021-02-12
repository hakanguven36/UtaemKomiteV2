using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UtaemKomiteV2.Models
{
	[Table("Dosya")]
	public class Dosya
	{
		public int ID { get; set; }

		[StringLength(600)]
		public string isim { get; set; }

		[StringLength(6)]
		public string uzantı { get; set; }

		[StringLength(600)]
		public string sysname { get; set; }

		public DateTime tarih { get; set; }

		public double boyut { get; set; }

		public bool silindi { get; set; }

		[StringLength(60)]
		public string kulName { get; set; }

		public DOSYATURU tur { get; set; }

		[StringLength(60)]
		public string icon { get; set; }
	}

	public enum DOSYATURU{
		Proje,
		Tutanak
	}
}
