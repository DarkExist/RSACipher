using Avalonia;

using Avalonia.ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using RSACipher.App.Interfaces;
using RSACipher.Frontend;
using RSACipher.Infrastructure;


//using Microsoft.Extensions.DependencyInjection;
using System;

namespace RSACipher.Front
{
	internal sealed class Program
	{
		private readonly IServiceCollection _services = new ServiceCollection();
		private readonly IServiceProvider _serviceProvider;

		public Program()
		{
			_services.AddService();
			_serviceProvider = _services.BuildServiceProvider();
		}

		public void Run(string[] args)
		{
			var ServiceControllerEx = new ServiceController(
				_serviceProvider.GetRequiredService<IReadFile>(),
				_serviceProvider.GetRequiredService<IWriteFile>());
			BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
		}

		// Initialization code. Don't use any Avalonia, third-party APIs or any
		// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
		// yet and stuff might break.
		//[STAThread]
		//public static void Main(string[] args) => BuildAvaloniaApp()
		//	.StartWithClassicDesktopLifetime(args);


		[STAThread]
		public static void Main(string[] args) 
		{
			var app = new Program();
			app.Run(args);
		}

		// Avalonia configuration, don't remove; also used by visual designer.
		public static AppBuilder BuildAvaloniaApp()
			=> AppBuilder.Configure<App>()
				.UsePlatformDetect()
				.WithInterFont()
				.LogToTrace()
				.UseReactiveUI();
	}
}
