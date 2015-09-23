using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Orion.Zeta.Methods.Dev.Shared;
using Orion.Zeta.Methods.Ui.Dev.Tools.MVVM;

namespace Orion.Zeta.ViewModels {
	public class MainViewModel : BaseViewModel, IModifiableStyleSetting {
		#region Fields
		private readonly Task _initialisationTask;
		private IItem _suggestion;
		private string _expression;
		private bool _isSearching;
		private readonly Timer _expressionSearchTimer;
		private DateTime _lastTimeStartSearching;
		private Zeta Zeta => Zeta.Instance;
		#endregion

		#region Constructor
		public MainViewModel() {
			ApplyDefaultStyleAndDesignValue();
			IsSearching = false;
			ExpressionAutoCompleteCommand = new RelayCommand(OnExpressionAutoCompleteCommand);
			ExpressionRunCommand = new RelayCommand(OnExpressionRunCommand);
			RunCommand = new RelayCommand(OnRunCommand);
			SelectSuggestionCommand = new RelayCommand(OnSelectSuggestionCommand);
			OpenSettingCommand = new RelayCommand(OnOpenSettingCommand);
			Suggestions = new ObservableCollection<IItem>();
			Suggestion = null;
			_expression = string.Empty;
			var delaySearching = new TimeSpan(0, 0, 0, 0, 250);
			_expressionSearchTimer = new Timer(delaySearching.TotalMilliseconds) { AutoReset = false };
			_expressionSearchTimer.Elapsed += (sender, args) => {
				StartSearching(Expression);
			};
			_initialisationTask = Zeta.InitialisationSearchEngineAsync();
			_isSettingOpen = false;
			Zeta.SettingWindowChanged += (sender, args) => {
				_isSettingOpen = args.Status == Zeta.SettingWindowChangedEventArgs.WindowStatus.Open;
				OnPropertyChanged("IsHideWhenLostFocus");
			};
		}
		#endregion

		#region Functionalities
		#region Properties
		#region Commands
		public ICommand ExpressionAutoCompleteCommand { get; set; }

		public ICommand ExpressionRunCommand { get; set; }

		public ICommand RunCommand { get; set; }

		public ICommand SelectSuggestionCommand { get; set; }

		public ICommand OpenSettingCommand { get; set; }
		#endregion

		public string Expression {
			get { return _expression; }
			set {
				_expression = value;
				OnExpressionUpdated();
			}
		}

		public IItem Suggestion {
			get { return _suggestion; }
			set {
				_suggestion = value;
				OnPropertyChanged();
			}
		}

		public bool IsSearching {
			get { return _isSearching; }
			set {
				_isSearching = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<IItem> Suggestions { get; set; }
		#endregion

		#region Events
		public event EventHandler OnAutoComplete;
		public event EventHandler OnProgramStart;
		public event EventHandler OnSearchFinished;
		#endregion

		#region Commands
		private void OnOpenSettingCommand() {
			Zeta.OpenSettingWindow();
		}

		private void OnSelectSuggestionCommand(object obj) {
			var item = obj as IItem;
			if (item != null) {
				Suggestion = item;
			}
		}

		private void OnRunCommand(object obj) {
			var item = obj as IItem;
			if (item != null) {
				if (item.IsValid()) {
					item.Execute.Start();
					Suggestions.Clear();
					OnProgramStart?.Invoke(this, new EventArgs());
				}
			}
		}

		private void OnExpressionRunCommand() {
			if (_suggestion == null || !_suggestion.IsValid()) return;
			_suggestion.Execute.Start();
			OnProgramStart?.Invoke(this, new EventArgs());
		}

		private void OnExpressionAutoCompleteCommand() {
			if (Suggestion == null || Expression.Equals(Suggestion.Value)) {
				return;
			}
			_expression = Suggestion.Value;
			OnPropertyChanged("Expression");
			Suggestions.Clear();
			OnAutoComplete?.Invoke(this, new EventArgs());
		}
		#endregion

		#region Search
		private void OnExpressionUpdated() {
			if (_expressionSearchTimer.Enabled) {
				_expressionSearchTimer.Stop();
				_expressionSearchTimer.Start();
			}
			else {
				_expressionSearchTimer.Start();
			}
			Suggestion = null;
		}

		private void StartSearching(string expression) {
			if (string.IsNullOrEmpty(expression)) {
				if (Suggestions.Any()) {
					Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
						Suggestions.Clear();
						Suggestion = null;
					}));
				}
				return;
			}
			_lastTimeStartSearching = DateTime.Now;
			IsSearching = true;
			if (!_initialisationTask.IsCompleted) {
				_initialisationTask.ContinueWith(
					(task =>
						Zeta.SearchEngine.Search(expression)
							.ContinueWith(t => SearchingCallback(t.Result, _lastTimeStartSearching))));
			}
			else {
				Zeta.SearchEngine.Search(expression)
					.ContinueWith(t => SearchingCallback(t.Result, _lastTimeStartSearching));
			}
		}

		private void SearchingCallback(IEnumerable<IItem> suggestions, DateTime timeStart) {
			if (timeStart != _lastTimeStartSearching) {
				return;
			}
			var sortedList = suggestions.ToList();
			sortedList.Sort((item1, item2) => item1.Rank < item2.Rank ? -1 : item1.Rank == item2.Rank ? 0 : 1);
			Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
				Suggestions.Clear();
				if (sortedList.Count > 1) {
					foreach (var suggestion in sortedList) {
						Suggestions.Add(suggestion);
					}
				}
				var best = sortedList.FirstOrDefault();
				Suggestion = best;
				IsSearching = false;
				OnSearchFinished?.Invoke(this, new EventArgs());
			}));
		}
		#endregion
		#endregion

		#region Style & Design
		#region IModifiableGeneralSetting
		private bool _isHideWhenLostFocus;
		private bool _isAlwaysOnTop;
		private bool _isSettingOpen;

		public bool IsHideWhenLostFocus {
			get {
				return !_isSettingOpen && _isHideWhenLostFocus;
			}
			set {
				_isHideWhenLostFocus = value;
				OnPropertyChanged();
			}
		}

		public bool IsAlwaysOnTop {
			get { return _isAlwaysOnTop; }
			set {
				_isAlwaysOnTop = value;
				OnPropertyChanged();
			}
		}



		#endregion

		private void ApplyDefaultStyleAndDesignValue() {
			UseNoneWindowStyle = false;
			Width = 800;
		}

		#region IModifiableStyleSetting
		private bool _useNoneWindowStyle;
		private double _width;

		/// <summary>
		/// Get or set WindowStyle None or not.
		/// </summary>
		public bool UseNoneWindowStyle {
			get { return _useNoneWindowStyle; }
			set {
				_useNoneWindowStyle = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Get or set width of application
		/// </summary>
		public double Width {
			get { return _width; }
			set {
				_width = value;
				OnPropertyChanged();
			}
		}
		#endregion
		#endregion
	}

	public interface IModifiableStyleSetting {
		bool UseNoneWindowStyle { get; set; }
		double Width { get; set; }
		bool IsHideWhenLostFocus { get; set; }
		bool IsAlwaysOnTop { get; set; }
	}
}