using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Avalonia.VisualTree;
using RSACipher.Front.ViewModels;
using System.IO;

namespace RSACipher.Front.Views
{
	public partial class RsaCipherView : UserControl
	{
		public RsaCipherView()
		{
			InitializeComponent();
		}

		private void OnGenerateKeyBtnClicked(object? sender, RoutedEventArgs e)
		{
			if (sender is Button button && button.DataContext is RsaCipherViewModel rsaCipher)
			{
				rsaCipher.GenerateKeys();
			}
		}

		private async void OnDownloadMessageBtnClicked(object? sender, RoutedEventArgs e)
		{
			if (sender is Button button && button.DataContext is RsaCipherViewModel rsaCipher)
			{

				var topLevel = TopLevel.GetTopLevel(this);

				var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
				{
					Title = "Open Text File",
					AllowMultiple = false,
					FileTypeFilter = new[] { FilePickerFileTypes.TextPlain }
				});

				if (files.Count >= 1)
				{
					await rsaCipher.DownloadMessageFromTxt(files);
				}

			}
		}

		private void OnUploadResultBtnClick(object? sender, RoutedEventArgs e)
		{
			if (sender is Button button && button.DataContext is RsaCipherViewModel rsaCipher)
				rsaCipher.WriteContentToTxt();
		}

		private void OnEncodeBtnClicked(object? sender, RoutedEventArgs e)
		{
			if (sender is Button button && button.DataContext is RsaCipherViewModel rsaCipher)
			{
				rsaCipher.EncodeMessage();
			}
		}

		private void OnDecodeBtnClicked(object? sender, RoutedEventArgs e)
		{
			if (sender is Button button && button.DataContext is RsaCipherViewModel rsaCipher)
			{
				rsaCipher.DecodeMessage();
			}
		}

		public void MessageTextBoxKeyPressedHandler(object? sender, KeyEventArgs e)
		{
			if (sender is TextBox textBox && textBox.DataContext is RsaCipherViewModel rsaCipher)
			{

				if (e.KeySymbol == null)
				{
					e.Handled = false;
				}

				if (rsaCipher.IsValidMessageSymbol(e.KeySymbol))
				{
					e.Handled = false;
				}
			}
		}

		public async void MessageTextBoxPasteHandler(object? sender, RoutedEventArgs e)
		{
			if (sender is TextBox textBox && textBox.GetVisualRoot() is TopLevel topLevel && textBox.DataContext is RsaCipherViewModel rsaCipher)
			{
				e.Handled = true;

				var clipboard = topLevel.Clipboard;

				var pastedText = await clipboard?.GetTextAsync();

				if (pastedText != null)
				{
					// Передаем текст во ViewModel для проверки
					string filteredText = rsaCipher.FilterAvailableSymbols(pastedText);

					// Обновляем UI на основном потоке
					Dispatcher.UIThread.Post(() =>
					{
						// Вставляем отфильтрованный текст в текстовое поле
						int selectionStart = textBox.SelectionStart;
						(int start, int end) = (textBox.SelectionStart, textBox.SelectionEnd);
						if (end < start)
							(start, end) = (end, start);
						textBox.Text = textBox.Text.Remove(start, end - start);
						textBox.Text = textBox.Text.Insert(start, filteredText);
					});
				}
			}
		}


		public void ResultTextBoxKeyDownHandler(object? sender, KeyEventArgs e)
		{
			//if (e.Key == Key.C)
			//{
			//	if (sender is TextBox textBox && textBox.GetVisualRoot() is TopLevel topLevel)
			//	{
			//		var clipboard = topLevel.Clipboard;
			//		clipboard.SetTextAsync(textBox.Text);
			//		e.Handled = true;

			//	}
			//}
		}
	}
}