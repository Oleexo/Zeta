using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Microsoft.TeamFoundation.MVVM;
using Orion.Zeta.Core;
using Orion.Zeta.Core.SearchMethods.ExplorerSearch;
using Orion.Zeta.Core.SearchMethods.Shared;

namespace Orion.Zeta.ViewModels {
	public class MainViewModel : BaseViewModel {
		public ICommand ExpressionAutoCompleteCommand { get; set; }

		public ICommand ExpressionRunCommand { get; set; }

		public event EventHandler OnAutoComplete;

		public string Expression {
			get { return this._expression.Value; }
			set {
				this._expression.Value = value;
				this.StartSearching(value);
			}
		}

		public IItem Suggestion {
			get { return this._suggestion; }
			set {
				this._suggestion = value;
				this.OnPropertyChanged();
			}
		}

		public ObservableCollection<IItem> Suggestions { get; set; }

		private IItem _suggestion;
		private readonly Lazy<SearchEngine> _searchEngine = new Lazy<SearchEngine>(InitialisationSearchEngine);
		private IItem _expression;

		public MainViewModel() {
			this.ExpressionAutoCompleteCommand = new RelayCommand(this.OnExpressionAutoCompleteCommand);
			this.ExpressionRunCommand = new RelayCommand(this.OnExpressionRunCommand);
			this.Suggestions = new ObservableCollection<IItem>();
			this.Suggestion = null;
			this._expression = new Item(@"~/Downloads/a", null);//string.Empty;
		}

		private void OnExpressionRunCommand() {
			if (this._expression.IsValid())
				this._expression.Execute.Start();
			else if (this._suggestion.IsValid())
				this._suggestion.Execute.Start();
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
			return searchEngine;
		}



		private SearchEngine SearchEngine {
			get { return this._searchEngine.Value; }
		}

		private void StartSearching(string expression) {
			if (String.IsNullOrEmpty(expression)) {
				return;
			}
			var suggestions = this.SearchEngine.Search(expression);
			this.Suggestions.Clear();
			if (suggestions.Count() > 1) {
				foreach (var suggestion in suggestions) {
					this.Suggestions.Add(suggestion);
					Console.WriteLine(suggestion.Value);
				}
			}
			var best = suggestions.FirstOrDefault();
			this.Suggestion = best;
		}
	}
}