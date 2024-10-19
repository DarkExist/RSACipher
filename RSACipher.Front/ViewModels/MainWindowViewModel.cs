using ReactiveUI;

namespace RSACipher.Front.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		public string Greeting { get; set; } = "aboba";

		private object content;

		public object Content
		{
			get => content;
			set
			{
				this.RaiseAndSetIfChanged(ref content, value);
			}
		}

		public MainWindowViewModel()
		{
			Content = new RsaCipherViewModel();
		}
	}
}
