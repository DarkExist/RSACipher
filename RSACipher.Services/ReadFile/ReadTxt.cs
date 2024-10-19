using RSACipher.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSACipher.Services.ReadFile
{
	public class ReadTxt : IReadFile
	{
		public byte[] Read(string pathToFile)
		{
			if (!File.Exists(pathToFile))
				throw new FileNotFoundException("No such file or cant access it");

			return Encoding.Unicode.GetBytes(File.ReadAllText(pathToFile));
		}

		public async Task<byte[]> ReadAsync(string pathToFile)
		{
			if (!File.Exists(pathToFile))
				throw new FileNotFoundException("No such file or cant access it");

			return Encoding.Unicode.GetBytes(await File.ReadAllTextAsync(pathToFile));
		}
	}
}
