using RSACipher.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSACipher.Services.RSACipher
{
	public static class KeyGenerator
	{
		static Random random = new Random();
		public static KeyPair GetRSAKeys(long numberOfDigitInN)
		{
			Tuple<BigInteger, BigInteger, BigInteger> PQNTuple = GetPandQandN(numberOfDigitInN);
			BigInteger P = PQNTuple.Item1, Q = PQNTuple.Item2, N = PQNTuple.Item3;
			//Console.WriteLine("PQN!");


			BigInteger d = GetEilerValue(P, Q);
			
			//Console.WriteLine("Eiler!");
			BigInteger s = GetSValue(d);
			//Console.WriteLine("S!");
			BigInteger e = GetEValue(s, d);
			//Console.WriteLine("E!");

			KeyPair keyPair = new KeyPair((s, N), (e, N));
			return keyPair;
		}

		private static Tuple<BigInteger, BigInteger, BigInteger> GetPandQandN(long numberOfDigitInN)
		{
			BigInteger P, Q, tempResult, result = (BigInteger)Math.Pow(10, numberOfDigitInN);
			do
			{
				(P, Q) = GeneratePrimesForLength(numberOfDigitInN);
				tempResult = P * Q;
				//Console.WriteLine($"{P.ToString()}, {Q.ToString()}, {tempResult}, {tempResult.ToString().Length}");
			} while (tempResult.ToString().Length != numberOfDigitInN || !IsPrime(P) || !IsPrime(Q));
			result = tempResult;

			return Tuple.Create(P, Q, result);
		}

		private static BigInteger GetEilerValue(BigInteger P, BigInteger Q)
		{
			return (P - 1) * (Q - 1);
		}

		private static BigInteger GetSValue(BigInteger d)
		{
			BigInteger randomValue;
			do {
				randomValue = GetRandomBigInteger(d);
				//Console.WriteLine($"d: {randomValue}");
			} while (!IsCoprime(randomValue, d));

			return randomValue;
		}

		private static BigInteger GetEValue(BigInteger s, BigInteger d)
		{
			BigInteger e = ModInverse(s, d);
			return e;
		}


		private static bool IsCoprime(BigInteger num1, BigInteger num2)
		{
			if (num2 == 0)
			{
				return num1 == 1;
			}

			return IsCoprime(num2, num1 % num2);
		}
	
		private static BigInteger GetRandomBigInteger(BigInteger n)
		{
			byte[] bytes = n.ToByteArray();
			
			random.NextBytes(bytes);

			BigInteger randomValue = new BigInteger(bytes);

			if (randomValue < 0)
			{
				randomValue = -randomValue;
			}

			bytes[bytes.Length - 1] &= (byte)0x7F; //force sign bit to positive
			return new BigInteger(bytes);
		}

		private static bool IsPrime(BigInteger n)
		{

			bool flag = true;
			for (BigInteger i = 2; i * i * i <= n; i++)
			{
				if (n % i == 0)
				{
					flag = false;
					break;
				}
			}
			return flag;
		}


		public static BigInteger ModInverse(BigInteger s, BigInteger d)
		{
			BigInteger t = 0, newT = 1;
			BigInteger r = d, newR = s;

			while (newR != 0)
			{
				BigInteger quotient = r / newR;

				// Обновляем t и r
				(t, newT) = (newT, t - quotient * newT);
				(r, newR) = (newR, r - quotient * newR);
			}

			// Если остаток r не 1, значит обратного элемента не существует
			if (r > 1)
				throw new ArgumentException("Обратный элемент не существует");

			// Результат положительный
			if (t < 0)
				t += d;

			return t;
		}

		public static (BigInteger P, BigInteger Q) GeneratePrimesForLength(long decimalLength)
		{
			int bits = (int)(Math.Floor(decimalLength / 1.8 * Math.Log2(10))); // Количество бит для P и Q
			BigInteger P = GenerateRandomPrime(bits);
			BigInteger Q = GenerateRandomPrime(bits);

			return (P, Q);
		}

		public static BigInteger GenerateRandomPrime(int bits)
		{
			Random rand = new Random();
			BigInteger prime;

			do
			{
				// Генерируем случайное число с указанным количеством бит
				byte[] bytes = new byte[bits / 8];
				rand.NextBytes(bytes);
				bytes[bytes.Length - 1] &= 0x7F; // Убедимся, что число положительное
				prime = new BigInteger(bytes);
			} while (!IsProbablyPrime(prime, 10)); // Проверка на простоту

			return prime;
		}

		// Тест простоты Миллера-Рабина
		private static bool IsProbablyPrime(BigInteger number, int k)
		{
			if (number < 2) return false;
			if (number != 2 && number % 2 == 0) return false;

			BigInteger d = number - 1;
			int r = 0;

			while (d % 2 == 0)
			{
				d /= 2;
				r++;
			}

			Random rand = new Random();
			for (int i = 0; i < k; i++)
			{
				BigInteger a = 2 + (BigInteger)(rand.NextDouble() * (double)(number - 4));
				if (!MillerTest(a, d, number, r))
					return false;
			}

			return true;
		}

		private static bool MillerTest(BigInteger a, BigInteger d, BigInteger n, int r)
		{
			BigInteger x = BigInteger.ModPow(a, d, n);
			if (x == 1 || x == n - 1)
				return true;

			for (int i = 1; i < r; i++)
			{
				x = BigInteger.ModPow(x, 2, n);
				if (x == 1) return false;
				if (x == n - 1) return true;
			}

			return false;
		}

	}
}
