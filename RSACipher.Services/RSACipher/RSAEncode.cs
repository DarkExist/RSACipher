using Org.BouncyCastle.Security;
using RSACipher.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSACipher.Services.RSACipher
{
	public static class RSAEncode
	{
		public static List<BigInteger> Encode(byte[] data, Tuple<BigInteger, BigInteger> publicKey, int lenOfSymbol, BigInteger amountOfBlocks)
		{
			List<int> intNumbersInUnicode = byteArrayToIntArray(data);
			(BigInteger S, BigInteger N) = publicKey;

			int sizeOfBlock = CountSizeOfBlock(data, N, amountOfBlocks);

			List<BigInteger> result = new List<BigInteger>();

			var chunks = intNumbersInUnicode.Chunk(sizeOfBlock);
			
			foreach (var chunk in chunks)
			{
				string tempCipher = "";
				for (int j = 0; j < chunk.Length; j++)
				{
					string stringByte = chunk[j].ToString();
					int amountOfLeadZerosRequired = lenOfSymbol - stringByte.Length % lenOfSymbol;
					if (amountOfLeadZerosRequired == lenOfSymbol)
						amountOfLeadZerosRequired = 0;
					stringByte = stringByte.Insert(0, MultiplyString("0", amountOfLeadZerosRequired));
					tempCipher += stringByte;
				}
				BigInteger block = BigInteger.Parse(tempCipher);
				BigInteger cipheredBlock = ModularExponentiation.RtLBinaryMethod(block, S, N);
				result.Add(cipheredBlock);
			}

			return result;
		}

		public static List<BigInteger> Encode(byte[] data, Tuple<BigInteger, BigInteger> publicKey)
		{
			return Encode(data, publicKey, 4, CountAmountOfBlocks());
		}

		public static int CountAmountOfBlocks(bool lowConnection = false)
		{
			if (lowConnection)
			{
				return 1000;
			}
			else
			{
				return 20;
			}
		}

		private static int CountSizeOfBlock(byte[] data, BigInteger N, BigInteger amountOfBlocks) {
			int probablySize = (int)Math.Ceiling((double)data.Length / (double)amountOfBlocks);

			if (probablySize == 0)
				throw new InvalidParameterException("Data is empty");

			return probablySize;

		}


		private static string MultiplyString(string str, BigInteger count)
		{
			string result = "";
			for (BigInteger i = 0; i < count; i++)
			{
				result += str;
			}
			return result;
		}

		private static List<int> byteArrayToIntArray(byte[] data)
		{
			// in 10 system
			// first byte is second
			// 2 byte
			List<int> result = new List<int>();

			for (int i = 0; i < data.Length; i+=2)
			{
				int firstByte = (int)data[i + 1];
				int secondByte = (int)data[i];

				string firstByteHex = firstByte.ToString("X");
				string secondByteHex = secondByte.ToString("X");

				string resultByte = firstByteHex + secondByteHex;

				int resultNumberInUnicode = Convert.ToInt32(resultByte, 16);
				result.Add(resultNumberInUnicode);
			}

			return result;
		}
	}
}
