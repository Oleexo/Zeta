using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.TeamFoundation.MVVM;
using Orion.Zeta.Core;
using Orion.Zeta.Core.SearchMethods.ApplicationSearch;
using Orion.Zeta.Core.SearchMethods.ExplorerSearch;
using Orion.Zeta.Core.SearchMethods.Shared;

namespace Orion.Zeta.ViewModels {
	public class MainViewModel : BaseViewModel {
		public ICommand ExpressionAutoCompleteCommand { get; set; }

		public ICommand ExpressionRunCommand { get; set; }

		public ICommand RunCommand { get; set; }

		public ICommand SelectSuggestionCommand { get; set; }

		public event EventHandler OnAutoComplete;
		public event EventHandler OnProgramStart;

		public string Expression {
			get { return this._expression.Value; }
			set {
				this._expression.Value = value;
				this.OnExpressionUpdated();
			}
		}

		public IItem Suggestion {
			get { return this._suggestion; }
			set {
				this._suggestion = value;
				this.OnPropertyChanged();
			}
		}

		public bool IsSearching {
			get { return this._isSearching; }
			set {
				this._isSearching = value;
				this.OnPropertyChanged();
			}
		}

		public ObservableCollection<IItem> Suggestions { get; set; }

		private IItem _suggestion;
		private readonly Lazy<SearchEngine> _searchEngine = new Lazy<SearchEngine>(InitialisationSearchEngine);
		private IItem _expression;
		private bool _isSearching;
		private readonly TimeSpan _delaySearching;
		private readonly Timer _expressionSearchTimer;

		public MainViewModel() {
			this.IsSearching = false;
			this.ExpressionAutoCompleteCommand = new RelayCommand(this.OnExpressionAutoCompleteCommand);
			this.ExpressionRunCommand = new RelayCommand(this.OnExpressionRunCommand);
			this.RunCommand = new RelayCommand(this.OnRunCommand);
			this.SelectSuggestionCommand = new RelayCommand(this.OnSelectSuggestion);
			this.Suggestions = new ObservableCollection<IItem>();
			this.Suggestion = null;
			this._expression = new Item();
			this._delaySearching = new TimeSpan(0, 0, 0, 0, 250);
			this._expressionSearchTimer = new Timer(this._delaySearching.TotalMilliseconds) {AutoReset = false};
			this._expressionSearchTimer.Elapsed += (sender, args) => {
				this.StartSearching(this.Expression);
			};
		}

		private void OnSelectSuggestion(object obj) {
			var item = obj as IItem;
			if (item != null) {
				this.Suggestion = item;
			}
		}

		private void OnRunCommand(object obj) {
			var item = obj as IItem;
			if (item != null) {
				if (item.IsValid()) {
					item.Execute.Start();
					this.Suggestions.Clear();
					if (this.OnProgramStart != null) {
						this.OnProgramStart(this, new EventArgs());
					}
				}
			}
		}

		private void OnExpressionRunCommand() {
			if (this._expression.IsValid()) {
				this._expression.Execute.Start();
				if (this.OnProgramStart != null) {
					this.OnProgramStart(this, new EventArgs());
				}
			}
			else if (this._suggestion.IsValid()) {
				this._suggestion.Execute.Start();
				if (this.OnProgramStart != null) {
					this.OnProgramStart(this, new EventArgs());
				}
			}
		}

		private void OnExpressionAutoCompleteCommand() {
			if (this.Suggestion == null || this.Expression.Equals(this.Suggestion.Value)) {
				return;
			}
			this._expression = this.Suggestion;
			this.OnPropertyChanged("Expression");
			this.Suggestions.Clear();
			if (this.OnAutoComplete != null) {
				this.OnAutoComplete(this, new EventArgs());
			}
		}

		private static SearchEngine InitialisationSearchEngine() {
			var searchEngine = new SearchEngine();
			searchEngine.RegisterMethod(new ExplorerSearchMethod());
			var applicationSearchMethod = new ApplicationSearchMethod();
			//applicationSearchMethod.RegisterPath(Environment.GetFolderPath(Environment.SpecialFolder.Programs), new List<string> {"*.exe", "*.lnk"});
			applicationSearchMethod.RegisterPath(Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms), new List<string> { "*.exe", "*.lnk" });
			searchEngine.RegisterMethod(applicationSearchMethod);
			return searchEngine;
		}



		private SearchEngine SearchEngine {
			get { return this._searchEngine.Value; }
		}

		private void OnExpressionUpdated() {
			if (this._expressionSearchTimer.Enabled) {
				this._expressionSearchTimer.Stop();
				this._expressionSearchTimer.Start();
			}
			else {
				this._expressionSearchTimer.Start();
			}
		}

		private void StartSearching(string expression) {
			if (String.IsNullOrEmpty(expression)) {
				return;
			}
			// TODO find better way to protect double execution of searchengine (time window / cancel and start)
			if (!this.IsSearching) {
				this.IsSearching = true;
				this.SearchEngine.Search(expression).ContinueWith(t => this.SearchingCallback(t.Result));
			}
		}

		private void SearchingCallback(IEnumerable<IItem> suggestions) {
			Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
				this.Suggestions.Clear();
				if (suggestions.Count() > 1) {
					foreach (var suggestion in suggestions) {
						this.Suggestions.Add(suggestion);
					}
				}
				var best = suggestions.FirstOrDefault();
				this.Suggestion = best;
				this.IsSearching = false;
			}));
		}
	}
}