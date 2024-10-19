using System;
using RSACipher.App.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSACipher.Services.RSACipher
{
	public static class RSADecode
	{
		public static byte[] Decode(List<BigInteger> message, Tuple<BigInteger, BigInteger> privateKey, int lenOfSymbol)
		{
			(BigInteger E, BigInteger N) = privateKey;
			List<BigInteger> result = new List<BigInteger>();
			string uncipheredBlocks = "", output = "";
			int amountOfLeadZerosRequired = 0;

			foreach (BigInteger value in message)
			{
				BigInteger decodedBlock = ModularExponentiation.RtLBinaryMethod(value, E, N);
				string stringDecodedBlock = decodedBlock.ToString();
				
				amountOfLeadZerosRequired = lenOfSymbol - stringDecodedBlock.Length % lenOfSymbol;
				if (amountOfLeadZerosRequired == lenOfSymbol)
					amountOfLeadZerosRequired = 0;
				
				uncipheredBlocks += decodedBlock.ToString().Insert(0, MultiplyString("0", amountOfLeadZerosRequired)); ;
			}

			amountOfLeadZerosRequired = uncipheredBlocks.Length % lenOfSymbol;
			if (amountOfLeadZerosRequired == lenOfSymbol)
				amountOfLeadZerosRequired = 0;
			uncipheredBlocks = uncipheredBlocks.Insert(0, MultiplyString("0", amountOfLeadZerosRequired));


			byte[] asd = new byte[4];
			for (int i = 0; i < uncipheredBlocks.Length; i += lenOfSymbol)
			{
				string temp = uncipheredBlocks.Substring(i, lenOfSymbol);
				long unicodeValue = Convert.ToInt32(temp, 10);
				char unicodeSymbol = (char)unicodeValue;
				output += unicodeSymbol.ToString();
			}
			return Encoding.Unicode.GetBytes(output);
		}

		public static byte[] Decode(List<BigInteger> message, Tuple<BigInteger, BigInteger> privateKey) 
		{
			return Decode(message, privateKey, 4);					
		}

		public static string MultiplyString(string str, BigInteger count)
		{
			string result = "";
			for (BigInteger i = 0; i < count; i++)
			{
				result += str;
			}
			return result;
		}
	}
}
