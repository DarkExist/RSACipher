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

			int sizeOfBlock = CountSizeOfBlock(intNumbersInUnicode, N, amountOfBlocks);

			List<BigInteger> result = new List<BigInteger>();

			var chunks = intNumbersInUnicode.Chunk(6);
			
			foreach (var chunk in chunks)
			{
				StringBuilder tempCipherBuilder = new StringBuilder();
				//string tempCipher = "";
				for (int j = 0; j < chunk.Length; j++)
				{
					string stringByte = chunk[j].ToString();
					int amountOfLeadZerosRequired = lenOfSymbol - stringByte.Length % lenOfSymbol;
					if (amountOfLeadZerosRequired == lenOfSymbol)
						amountOfLeadZerosRequired = 0;
					stringByte = stringByte.Insert(0, MultiplyString("0", amountOfLeadZerosRequired));
					tempCipherBuilder.Append(stringByte);
				}
				BigInteger block = BigInteger.Parse(tempCipherBuilder.ToString());
				BigInteger cipheredBlock = ModularExponentiation.RtLBinaryMethod(block, S, N);
				result.Add(cipheredBlock);
				tempCipherBuilder.Clear();
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

		private static int CountSizeOfBlock(List<int> data, BigInteger N, BigInteger amountOfBlocks) {
			int probablySize = (int)Math.Ceiling((double)data.Count / (double)amountOfBlocks);

			if (probablySize == 0)
				throw new InvalidParameterException("Data is empty");

			return probablySize;

		}


		private static string MultiplyString(string str, BigInteger count)
		{
			return new StringBuilder().Insert(0, str, (int)count).ToString();
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
