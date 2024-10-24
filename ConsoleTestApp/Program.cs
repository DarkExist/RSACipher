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

		public static List<string> Alphabet { get; private set; } = new();
		public static void AlphabetFill()
		{
			for (int i = 33; i <= 126; i++)
			{
				/*if (i == '\\')
					continue;*/
				Alphabet.Add(((char)i).ToString());
			}

			for (int i = 1040; i <= 1103; i++)
			{
				Alphabet.Add(((char)i).ToString());
			}
			Alphabet.Add(((char)1025).ToString());
			Alphabet.Add(((char)1105).ToString());
		}

		static void Main(string[] args)
		{
			/*var as111111111d = (char)1088;

			Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

			Console.WriteLine(File.Exists("C:\\Users\\relog\\Downloads\\asd.txt"));


			DateTime start = DateTime.Now;
			KeyPair keyPair = KeyGenerator.GetRSAKeys(33);
			DateTime end = DateTime.Now;
			Console.WriteLine(end - start);
			Console.WriteLine(keyPair);

			var asd = "涁ABa We, я!涁ꀩힰ";
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


			Console.WriteLine(end - start);*/
			AlphabetFill();
			int lowerBorder = 1, upperBorder = 1000000, form = 10;
			Dictionary<int, TimeSpan> perfomanceEncodeResults = new();
			Dictionary<int, TimeSpan> perfomanceDecodeResults = new();

			for (int i = lowerBorder; i < upperBorder; i *= form)
			{
				DateTime start, end, start1, end1;
				
				string randomString = GenerateRandomString(i);
				byte[] bytes = Encoding.Unicode.GetBytes(randomString);
				List<BigInteger> numbers = new List<BigInteger>();

				KeyPair keyPair = KeyGenerator.GetRSAKeys(33);
				(BigInteger S, BigInteger N) = keyPair.PublicKey;
				(BigInteger E, N) = keyPair.PrivateKey;

				start = DateTime.Now;
				numbers = RSAEncode.Encode(bytes, Tuple.Create(S, N));
				end = DateTime.Now;

				start1 = DateTime.Now;
				RSADecode.Decode(numbers, Tuple.Create(E, N));
				end1 = DateTime.Now;

				perfomanceEncodeResults[i] = end - start;
				perfomanceDecodeResults[i] = end1 - start1;
				Console.WriteLine($"Encoding of message with length {i} takes {end - start}");
				Console.WriteLine($"DEcoding of message with length {i} takes {end1 - start1}");
			}

			Console.WriteLine("Результаты кодирования");
			foreach (KeyValuePair<int, TimeSpan> keyValuePair in perfomanceEncodeResults)
			{
				Console.WriteLine(keyValuePair.ToString());
			}

			Console.WriteLine("Результаты декодирования");
			foreach (KeyValuePair<int, TimeSpan> keyValuePair in perfomanceDecodeResults)
			{
				Console.WriteLine(keyValuePair.ToString());
			}
		}

		public static string GenerateRandomString(int length)
		{
			Random random = new Random();
			string randomString = new string(Enumerable.Repeat(0, length)
				.Select(_ => Alphabet[random.Next(Alphabet.Count)][0])
				.ToArray());

			return randomString;
		}
	}
}
