using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
			this._mainViewModel.OnAutoComplete += this.MainViewModelOnAutoComplete;
			this._mainViewModel.OnProgramStart += this.MainViewModelOnOnProgramStart;
			this.ExpressionTextBox.Focus();
		}

		private void MainViewModelOnOnProgramStart(object sender, EventArgs eventArgs) {
			// TODO minimize
		}

		private void MainViewModelOnAutoComplete(object sender, EventArgs e) {
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

		private void ExpressionTextBox_OnKeyUp(object sender, KeyEventArgs e) {
			switch (e.Key) {
				case Key.Down:
					if (this.SuggestionsListBox.SelectedIndex < this.SuggestionsListBox.Items.Count) {
						++this.SuggestionsListBox.SelectedIndex;
						this.SuggestionsListBox.UpdateLayout();
						this._mainViewModel.SelectSuggestionCommand.Execute(this.SuggestionsListBox.SelectedItem);
					}
					break;
				case Key.Up:
					if (this.SuggestionsListBox.SelectedIndex > 0) {
						--this.SuggestionsListBox.SelectedIndex;
						this.SuggestionsListBox.UpdateLayout();
						this._mainViewModel.SelectSuggestionCommand.Execute(this.SuggestionsListBox.SelectedItem);
					}
					break;
			}
		}

		private void SuggestionsListBox_OnKeyUp(object sender, KeyEventArgs e) {
			if (e.Key == Key.Enter) {
				this._mainViewModel.RunCommand.Execute(this.SuggestionsListBox.SelectedItem);
			}
		}
	}
}
