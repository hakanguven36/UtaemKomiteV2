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
			string path = @"pdflogo.png";
			string outputfile = @"pdflogoEnc.png";
			var byts = File.ReadAllBytes(path);
			string hata = AES.EncryptFile(byts, outputfile);
			Console.WriteLine("Hata: " + hata);
			Console.ReadKey();
		}
	}

	public static class AES
	{
		public static string EncryptFile(byte[] buffer, string outputFile)
		{
			Console.WriteLine("BufferBoyutu: " + buffer.Length);

			try
			{
				UnicodeEncoding UE = new UnicodeEncoding();
				byte[] key = UE.GetBytes("MyKEY001");
				RijndaelManaged RMCrypto = new RijndaelManaged();
				
				FileStream outputFS = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
				CryptoStream cs = new CryptoStream(outputFS, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write);

				cs.Write(buffer, 0, buffer.Length);
				
				cs.Close();
				outputFS.Close();
				return "Tamam";
			}
			catch (Exception e)
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
}
