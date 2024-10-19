using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Microsoft.Extensions.DependencyInjection;
using RSACipher.Services.ReadFile;
using RSACipher.Services.WriteFile;
using RSACipher.Services.RSACipher;
using ReactiveUI;
using RSACipher.App.Interfaces;
using Tmds.DBus.Protocol;
using System.Text.RegularExpressions;
using System.Linq;
using Avalonia.Input;
using Avalonia.Controls;
using ErrorOr;
using RSACipher.Core.Models;

namespace RSACipher.Front.ViewModels
{
	public class RsaCipherViewModel : ViewModelBase
	{
		private readonly ReadTxt _txtReader = new ReadTxt();
		private readonly WriteTxt _txtWriter = new WriteTxt();
		private string _publicKey = "";
		private string _privateKey = "";
		private string _message = "";
		private string _result = "";
		private Tuple<BigInteger, BigInteger> _publicKeyTuple;
		private Tuple<BigInteger, BigInteger> _privateKeyTuple;

		private bool IsCorrectKey = false;

		public string PublicKey
		{
			get => _publicKey;
			set => this.RaiseAndSetIfChanged(ref _publicKey, value);
		}
		public string PrivateKey
		{
			get => _privateKey;
			set => this.RaiseAndSetIfChanged(ref _privateKey, value);
		}

		public string Message
		{
			get => _message;
			set => this.RaiseAndSetIfChanged(ref _message, value);
		}

		public string Result
		{
			get => _result;
			set => this.RaiseAndSetIfChanged(ref _result, value);
		}

		private string availableSymbols = new string(Enumerable.Range(32, 126 - 33 + 1)
			.Concat(Enumerable.Range(1040, 1103 - 1040 + 1))
			.Concat(new[] { 1025, 1105 })
			.Select(i => (char)i)
			.ToArray());

		private int index = 0;
		public void GenerateKeys()
		{
			KeyPair generatedKeys = KeyGenerator.GetRSAKeys(33);
			_publicKeyTuple = Tuple.Create(generatedKeys.PublicKey.PublicExponent, generatedKeys.PublicKey.Modulus);
			_privateKeyTuple = Tuple.Create(generatedKeys.PrivateKey.PrivateExponent, generatedKeys.PrivateKey.Modulus);

			PublicKey = _publicKeyTuple.ToString();
			PrivateKey = _privateKeyTuple.ToString();
		}

		public async Task<string> GetContentFromTxt(IReadOnlyList<IStorageFile> files)
		{

			string filePath = files[0].Path.LocalPath;

			byte[] byteFile = await _txtReader.ReadAsync(filePath);

			string result = Encoding.Unicode.GetString(byteFile);
			return result;
		}

		public async Task DownloadMessageFromTxt(IReadOnlyList<IStorageFile> files)
		{
			string fileData = await GetContentFromTxt(files);
			Message = FilterAvailableSymbols(fileData);
		}

		public async void WriteContentToTxt()
		{
			byte[] data = Encoding.Unicode.GetBytes(Result);
			string currentTime = DateTime.Now.ToString("dd.MM.yy.HH.mm.ss.fff");
			var asd = AppDomain.CurrentDomain.BaseDirectory;
			await _txtWriter.WriteAsync(AppDomain.CurrentDomain.BaseDirectory + @"\" + $"{currentTime}.txt", data);


		}

		public void EncodeMessage()
		{
			var result = ParseKey(PublicKey);

			if (result.IsError)
			{
				Result = "Ошибка! Неверный публичный ключ! / Error! Incorrect public key!";
				return;
			}

			_publicKeyTuple = result.Value;

			try
			{
				byte[] byteMessage = Encoding.Unicode.GetBytes(Message);
				List<BigInteger> encodedMessage = RSAEncode.Encode(byteMessage, _publicKeyTuple);
				Result = string.Join(" ", encodedMessage);
			}
			catch
			{
				Result = "Ошибка! Проверьте поле Message / Error! Check the field Message";
			}

		}

		public void DecodeMessage()
		{
			var result = ParseKey(PrivateKey);

			if (result.IsError)
			{
				Result = "Ошибка! Неверный приватный ключ! / Error! Incorrect private key!";
				return;
			}

			_privateKeyTuple = result.Value;

			try
			{
				List<BigInteger> encodedMessage = Message.Split(" ").ToArray().Select(BigInteger.Parse).ToList();
				byte[] byteResult = RSADecode.Decode(encodedMessage, _privateKeyTuple);
				Result = Encoding.Unicode.GetString(byteResult);
			}
			catch
			{
				Result = "Ошибка! Проверьте поле Message / Error! Check the field Message";
			}
		}

		public string FilterAvailableSymbols(string message)
		{
			return new string(message.Where(i => IsValidMessageSymbol(i.ToString())).ToArray());
		}

		public bool IsValidMessageSymbol(string symbol)
		{
			if (symbol == null) return false;
			return availableSymbols.Contains(symbol);
		}

		public bool IsValidKey(string key, bool isPrivateKey)
		{

			var result = ParseKey(key);

			if (result.IsError) return false;

			return true;
		}

		private ErrorOr<Tuple<BigInteger, BigInteger>> ParseKey(string key)
		{
			string regexForm = "\\((\\d+), (\\d+)\\)";
			Regex reg = new Regex(@regexForm);
			Match m = reg.Match(key);
			if (m.Success)
				return Tuple.Create(BigInteger.Parse(m.Groups[1].Value), BigInteger.Parse(m.Groups[2].Value));
			else
				return new Error();
		}


	}
}