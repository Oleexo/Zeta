using System;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Orion.Zeta.ViewModels;

namespace Orion.Zeta {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow {
		private readonly MainViewModel _mainViewModel;

		public MainWindow() {
			this.InitializeComponent();
			this.AllowsTransparency = true;
			this._mainViewModel = new MainViewModel();
			this.DataContext = this._mainViewModel;
			this._mainViewModel.OnAutoComplete += this.OnAutoComplete;
		}

		private void OnAutoComplete(object sender, EventArgs e) {
			this.ExpressionTextBox.CaretIndex = this.ExpressionTextBox.Text.Length;
		}

		private void ExpressionTextBox_OnScrollChanged(object sender, ScrollChangedEventArgs e) {
			if (e.HorizontalOffset >= (e.ExtentWidth - e.ViewportWidth) - 1) {
				if (this.SuggestionTextBox.Visibility != Visibility.Visible)
					this.SuggestionTextBox.Visibility = Visibility.Visible;
				var margin = this.SuggestionTextBox.Margin;
				margin.Left = -(this.ExpressionTextBox.HorizontalOffset);
				this.SuggestionTextBox.Margin = margin;
			}
			else {
				if (this.SuggestionTextBox.Visibility == Visibility.Visible) {
					this.SuggestionTextBox.Visibility = Visibility.Hidden;
				}
			}
		}
	}
}
