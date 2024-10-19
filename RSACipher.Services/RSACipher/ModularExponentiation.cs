using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSACipher.Services.RSACipher
{
	public class ModularExponentiation
	{
		public static BigInteger RtLBinaryMethod(BigInteger baseValue, BigInteger exponent, BigInteger modulus)
		{
			if (modulus == 1) return 0;

			BigInteger result = 1;

			baseValue = baseValue % modulus;

			while (exponent > 0)
			{
				// Если exponent нечетный, умножаем результат на baseValue
				if ((exponent % 2) == 1)
				{
					result = (result * baseValue) % modulus;
				}


				exponent = exponent >> 1; 
				baseValue = (baseValue * baseValue) % modulus;
			}

			return result;
		}
	}
}
