using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSACipher.App.Interfaces
{
	public interface IWriteFile
	{
		public Task<string> WriteAsync(string pathToFile, byte[] data);
		public string Write(string pathToFile, byte[] data);
	}
}
