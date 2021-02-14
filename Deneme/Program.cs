using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Security.Cryptography;
using System.Text;


//IsolatedStorageFileStream iss;
namespace Deneme
{
	class Program
	{
		static void Main(string[] args)
		{
			MemoryStream ms2 = new MemoryStream();

			string path = @"text2.txt";
			string path2 = @"textEnc.txt";
			using (FileStream fs1 = new FileStream(path, FileMode.Open))
			using (FileStream fs2 = new FileStream(path2, FileMode.Create))
			using(MemoryStream ms1 = new MemoryStream())
			{
				fs1.CopyTo(ms1);

				SIFRELEME sifreleme = new SIFRELEME();
				ms2 = sifreleme.Kilitle(ms1);
				ms2.Position = 0;
				ms2.CopyTo(fs2);
				fs2.Position = 0;
				fs2.Flush();
			}

			ms2.Dispose();
			Console.WriteLine("Tamamlandı");
			Console.ReadKey();
		}
	}

	public class SIFRELEME
	{
		public MemoryStream Kilitle(MemoryStream input)
		{
			RijndaelManaged RMCrypto = new RijndaelManaged();
			var key = new UnicodeEncoding().GetBytes("MyKEY001");
			using (CryptoStream cs = new CryptoStream(input, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Read))
			{
				MemoryStream output = new MemoryStream();
				input.Position = 0;
				cs.CopyTo(output);
				return output;
			}
		}

		public MemoryStream KilitAç(MemoryStream input)
		{
			RijndaelManaged RMCrypto = new RijndaelManaged();
			var key = new UnicodeEncoding().GetBytes("MyKEY001");
			using (CryptoStream cs = new CryptoStream(input, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read))
			{
				MemoryStream output = new MemoryStream();
				input.Position = 0;
				cs.CopyTo(output);
				return output;
			}
		}
	}
}
