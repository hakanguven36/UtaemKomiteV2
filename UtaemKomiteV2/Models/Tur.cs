using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UtaemKomiteV2.Models
{
	[Table("Tur")]
	public class Tur
	{
		public int ID { get; set; }

		[StringLength(60)]
		public string isim { get; set; }

		public IEnumerable<Dosya> dosyalar { get; set; }
	}
}
