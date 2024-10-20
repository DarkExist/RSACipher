using Avalonia.Controls;
using Avalonia.Interactivity;

namespace RSACipher.Front.Views
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

		}

		protected override void OnLoaded(RoutedEventArgs e)
		{
			base.OnLoaded(e);
			MinWidth = 1180;
			MinHeight = Height;
			SizeToContent = SizeToContent.Manual;
		}
	}
}