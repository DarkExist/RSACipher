namespace RSACipher.App.Interfaces
{
	public interface IReadFile
	{
		public Task<byte[]> ReadAsync(string pathToFile);
		public byte[] Read(string pathToFile);
	}
}
