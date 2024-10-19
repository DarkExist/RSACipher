using RSACipher.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSACipher.Services.WriteFile
{
	public class WriteTxt : IWriteFile
	{
		public string Write(string pathToFile, byte[] data)
		{
			if (!Directory.Exists(pathToFile))
				throw new FileNotFoundException("No such file or cant access it");

			File.WriteAllBytes(pathToFile, data);
			return pathToFile;
		}

		public async Task<string> WriteAsync(string pathToFile, byte[] data)
		{
			string stringData = Encoding.Unicode.GetString(data);

			await File.WriteAllTextAsync(pathToFile, stringData);

			return pathToFile;
		}
	}
}
