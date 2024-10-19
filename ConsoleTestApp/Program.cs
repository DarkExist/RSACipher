using Microsoft.Extensions.DependencyInjection;
using RSACipher.App.Interfaces;
using RSACipher.Core.Models;
using RSACipher.Infrastructure;
using RSACipher.Services.ReadFile;
using RSACipher.Services.RSACipher;
using RSACipher.Services.WriteFile;
using System;
using System.Numerics;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace ConsoleTestApp
{
	internal class Program
	{
		private IServiceCollection _services = new ServiceCollection();
		private IServiceProvider _serviceProvider;

		public Program()
		{
			_services.AddService();
			_serviceProvider = _services.BuildServiceProvider();
		}

		public void Run()
		{

			var as111111111d = (char)1088;

			Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

			var cccEx = new ccc(_serviceProvider.GetRequiredService<IReadFile>(),
								_serviceProvider.GetRequiredService<IWriteFile>());

			Console.WriteLine(File.Exists("C:\\Users\\relog\\Downloads\\asd.txt"));


			DateTime start = DateTime.Now;
			KeyPair keyPair = KeyGenerator.GetRSAKeys(33);
			DateTime end = DateTime.Now;
			Console.WriteLine(end - start);
			Console.WriteLine(keyPair);

			var asd = "涁ABoba We, Друзья!涁ꀩힰ";
			byte[] bytes = Encoding.UTF32.GetBytes(asd);
			foreach (byte b in bytes)
			{
				Console.WriteLine(b);
			}

			(BigInteger s, BigInteger n) = keyPair.PublicKey;
			(BigInteger e, n) = keyPair.PrivateKey;

			byte[] bytes1 = Encoding.Unicode.GetBytes(asd);

			var asdd = Encoding.Unicode.GetBytes(asd);
			var encodeRes = RSAEncode.Encode(asdd, Tuple.Create(s, n), 5, 20);

			var decodeRes = RSADecode.Decode(encodeRes, Tuple.Create(e, n), 5);
			var decodeString = Encoding.Unicode.GetString(decodeRes);
			int vv = asd[0];
			string bb = vv.ToString().Insert(0, "00");

			BigInteger a = 2650027;
			//(BigInteger S, BigInteger N) = keyPair.PublicKey;
			//(BigInteger E, N) = keyPair.PrivateKey;

			(BigInteger E, BigInteger N) = (BigInteger.Parse("80317660540871758015278937435111"), BigInteger.Parse("280335740142224458779324508377521"));
			BigInteger c = 1;



			List<BigInteger> numbers = new List<BigInteger>();
			BigInteger AB = BigInteger.Parse("203947401696862619333112415464771");
			numbers.Add(AB);

			var dddd = ModularExponentiation.RtLBinaryMethod(AB, E, N);

			var asddd = RSADecode.Decode(numbers, Tuple.Create(E, N));


			start = DateTime.Now;




			end = DateTime.Now;


			Console.WriteLine(end - start);
		}

		static void Main(string[] args)
		{
			var app = new Program();
			app.Run();
			
		}
	}
}
