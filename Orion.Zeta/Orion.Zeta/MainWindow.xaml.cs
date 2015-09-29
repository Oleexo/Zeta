using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Orion.Zeta.ViewModels;

namespace Orion.Zeta {
	public partial class MainWindow : MetroWindow {
		private readonly MainViewModel _mainViewModel;
		private bool _isFirstTime;

		public MainWindow() {
			InitializeComponent();
			AllowsTransparency = true;
			_mainViewModel = new MainViewModel();
			DataContext = _mainViewModel;
			_mainViewModel.OnAutoComplete += MainViewModelOnAutoComplete;
			_mainViewModel.OnProgramStart += MainViewModelOnOnProgramStart;
			_mainViewModel.OnSearchFinished += MainViewModelOnOnSearchFinished;
			ExpressionTextBox.Focus();
			Deactivated += OnDeactivated;
			_isFirstTime = true;
			MinimizeApplication();
		}

		private void OnDeactivated(object sender, EventArgs eventArgs) {
			if (_mainViewModel.IsHideWhenLostFocus)
				MinimizeApplication();
		}

		public void WakeUpApplication() {
			if (WindowState == WindowState.Normal) {
				return;
			}
			Show();
			WindowState = WindowState.Normal;
			ExpressionTextBox.Focus();
			ExpressionTextBox.SelectAll();
			CenterInScreen();
		}

		private void CenterInScreen() {
			Top = 0;
			var width = ActualWidth;
			if (_isFirstTime) {
				if (!double.IsNaN(Width)) { width = Width; }
				_isFirstTime = false;
			}
			Left = (SystemParameters.WorkArea.Width - width) / 2 + SystemParameters.WorkArea.Left;
		}

		private void MainViewModelOnOnSearchFinished(object sender, EventArgs eventArgs) {
			SuggestionsListBox.SelectedIndex = 0;
			SuggestionTextBox.UpdateLayout();
			_mainViewModel.SelectSuggestionCommand.Execute(SuggestionsListBox.SelectedItem);
		}

		private void MainViewModelOnOnProgramStart(object sender, EventArgs eventArgs) {
			MinimizeApplication();
		}

		private void MinimizeApplication() {
			WindowState = WindowState.Minimized;
			Hide();
		}

		private void MainViewModelOnAutoComplete(object sender, EventArgs e) {
			ExpressionTextBox.CaretIndex = ExpressionTextBox.Text.Length;
		}

		private void ExpressionTextBox_OnScrollChanged(object sender, ScrollChangedEventArgs e) {
			if (e.HorizontalOffset >= (e.ExtentWidth - e.ViewportWidth) - 1) {
				if (SuggestionTextBox.Visibility == Visibility.Hidden)
					SuggestionTextBox.Visibility = Visibility.Visible;
				var margin = SuggestionTextBox.Margin;
				margin.Left = -(ExpressionTextBox.HorizontalOffset);
				SuggestionTextBox.Margin = margin;
			}
			else {
				if (SuggestionTextBox.Visibility == Visibility.Visible) {
					SuggestionTextBox.Visibility = Visibility.Hidden;
				}
			}
		}

		private void ExpressionTextBox_OnKeyUp(object sender, KeyEventArgs e) {
			switch (e.Key) {
				case Key.Down:
					if (SuggestionsListBox.SelectedIndex < SuggestionsListBox.Items.Count) {
						++SuggestionsListBox.SelectedIndex;
						SuggestionsListBox.UpdateLayout();
						SuggestionTextBox.BringIntoView();
						_mainViewModel.SelectSuggestionCommand.Execute(SuggestionsListBox.SelectedItem);
					}
					break;
				case Key.Up:
					if (SuggestionsListBox.SelectedIndex > 0) {
						--SuggestionsListBox.SelectedIndex;
						SuggestionsListBox.UpdateLayout();
						SuggestionTextBox.BringIntoView();
						_mainViewModel.SelectSuggestionCommand.Execute(SuggestionsListBox.SelectedItem);
					}
					break;
				case Key.Escape:
					MinimizeApplication();
					break;
			}
		}

		private void SuggestionsListBox_OnKeyUp(object sender, KeyEventArgs e) {
			if (e.Key == Key.Enter) {
				_mainViewModel.RunCommand.Execute(SuggestionsListBox.SelectedItem);
			}
		}

		private void MainWindow_OnStateChanged(object sender, EventArgs e) {
			if (WindowState == WindowState.Minimized) {
				MinimizeApplication();
			}
		}

		private void SuggestionTextBox_OnTextChanged(object sender, TextChangedEventArgs e) {
			if (!SuggestionTextBox.Text.StartsWith(ExpressionTextBox.Text, StringComparison.OrdinalIgnoreCase)) {
				SuggestionTextBox.Visibility = Visibility.Collapsed;
			}
			else {
				if (SuggestionTextBox.Visibility == Visibility.Collapsed) {
					SuggestionTextBox.Visibility = Visibility.Visible;
				}
			}
		}

		private void SuggestionsListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
			var selector = sender as Selector;
			(selector as ListBox)?.ScrollIntoView(selector.SelectedItem);
		}
	}
}
