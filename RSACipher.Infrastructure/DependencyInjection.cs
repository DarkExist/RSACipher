using Microsoft.Extensions.DependencyInjection;
using RSACipher.Services.ReadFile;
using RSACipher.App.Interfaces;
using RSACipher.Services.WriteFile;

namespace RSACipher.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddService(this IServiceCollection services) 
		{
			services.AddTransient<IReadFile, ReadTxt>();
			services.AddTransient<IWriteFile, WriteTxt>();
			return services;
		}
	}
}
