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
		private bool _changedExpression;

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

		private void SuggestionTextBox_OnScrollChanged(object sender, ScrollChangedEventArgs e) {
			if (!this._changedExpression) {
				return;
			}
			if (this.ExpressionTextBox.HorizontalOffset > e.HorizontalOffset - 1 && this.SuggestionTextBox.Text.Length > 0) {
				var padding = this.SuggestionTextBox.Padding;
				padding.Right = this.ExpressionTextBox.HorizontalOffset - this.SuggestionTextBox.HorizontalOffset;
				this.SuggestionTextBox.Padding = padding;
				this.SuggestionTextBox.ScrollToHorizontalOffset(this.ExpressionTextBox.HorizontalOffset);
			}
			if (this.SuggestionTextBox.Text.Length == 0) {
				this.ResetSuggestionPadding();
			}
			this._changedExpression = false;
		}

		private void ExpressionTextBox_OnScrollChanged(object sender, ScrollChangedEventArgs e) {
			this._changedExpression = true;
			if (e.HorizontalOffset >= (e.ExtentWidth - e.ViewportWidth) - 1) {
				this.ResetSuggestionPadding();
				if (this.SuggestionTextBox.Visibility != Visibility.Visible)
					this.SuggestionTextBox.Visibility = Visibility.Visible;
				this.SuggestionTextBox.ScrollToHorizontalOffset(this.ExpressionTextBox.HorizontalOffset);
			}
			else {
				if (this.SuggestionTextBox.Visibility == Visibility.Visible) {
					this.ResetSuggestionPadding();
					this.SuggestionTextBox.Visibility = Visibility.Hidden;
				}
			}
		}

		private void ResetSuggestionPadding() {
			var padding = this.SuggestionTextBox.Padding;
			padding.Right = 0;
			this.SuggestionTextBox.Padding = padding;
		}
	}
}
