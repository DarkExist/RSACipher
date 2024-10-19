using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSACipher.Core.Models
{
	public class KeyPair
	{
		public (BigInteger PublicExponent, BigInteger Modulus) PublicKey { get; set; }
		public (BigInteger PrivateExponent, BigInteger Modulus) PrivateKey { get; set; }

		public KeyPair((BigInteger publicExponent, BigInteger modulus) publicKey, (BigInteger privateExponent, BigInteger modulus) privateKey)
		{
			PublicKey = publicKey;
			PrivateKey = privateKey;
		}

		public override string ToString()
		{
			return $"Public:({PublicKey.PublicExponent},{PublicKey.Modulus}),Private:({PrivateKey.PrivateExponent},{PrivateKey.Modulus})";
		}
	}
}
