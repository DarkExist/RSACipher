using RSACipher.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSACipher.Frontend
{
	public class ServiceController
	{
		private readonly IReadFile readFile;
		private readonly IWriteFile writeFile;
		public ServiceController(IReadFile readFile, IWriteFile writeFile)
		{
			this.readFile = readFile;
			this.writeFile = writeFile;
		}

		public async Task<byte[]> ReadAsync(string pathToFile)
		{
			return await readFile.ReadAsync(pathToFile);
		}

		public async Task<string> WriteAsync(string pathToFile, byte[] data)
		{
			return await writeFile.WriteAsync(pathToFile, data);
		}
	}
}
