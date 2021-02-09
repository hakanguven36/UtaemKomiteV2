using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using UtaemKomiteV2.Araclar;

namespace UtaemKomiteV2.Models
{
	[Table("Kullar")]
	public class Kullar
	{
		public int ID { get; set; }

		[Required]
		[StringLength(30, MinimumLength = 4, ErrorMessage = "4-30 Karakter olmalı!")]
		public string kulname { get; set; }

		private string _kulpass;
		[StringLength(600, MinimumLength = 4, ErrorMessage = "4-12 Karakter olmalı!")]
		[Encrypted(nameof(_kulpass))]
		public string kulpass
		{
			get => _kulpass.encout();
			set => _kulpass = value.encin();
		}


		public bool hatirla { get; set; }

		private string _cerez;
		[StringLength(600, MinimumLength = 4, ErrorMessage = "4-12 Karakter olmalı!")]
		[Encrypted(nameof(_cerez))]
		public string cerez
		{
			get => _cerez.encout();
			set => _cerez = value.encin();
		}

		public int hatali { get; set; }

		public DateTime kilitliTarih { get; set; }

		public bool admin { get { return kulname.encin() == adminCode; } set { if (value) adminCode = kulname.encin(); else adminCode = ""; } }

		[StringLength(600)]
		public string adminCode { get; set; }

	}


	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	sealed class EncryptedAttribute : Attribute
	{
		readonly string _fieldName;

		public EncryptedAttribute(string fieldName)
		{
			_fieldName = fieldName;
		}

		public string FieldName => _fieldName;
	}
}
