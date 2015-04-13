using System;
using System.Linq;
using System.Windows.Input;
using Microsoft.TeamFoundation.MVVM;
using Orion.Zeta.Core;
using Orion.Zeta.Core.SearchMethods.ExplorerSearch;

namespace Orion.Zeta.ViewModels {
	public class MainViewModel : BaseViewModel {
		public ICommand ExpressionAutoCompleteCommand { get; set; }

		public string Expression {
			get { return this._expression; }
			set {
				this._expression = value;
				this.StartSearching();
			}
		}

		public string Suggestion {
			get { return this._suggestion; }
			set {
				this._suggestion = value;
				this.OnPropertyChanged();
			}
		}

		public int ExpressionCaretIndex { get; set; }

		private string _suggestion;
		private readonly Lazy<SearchEngine> _searchEngine = new Lazy<SearchEngine>(InitialisationSearchEngine);
		private string _expression;

		public MainViewModel() {
			this.Expression = string.Empty;
			this.Suggestion = string.Empty;
			this.ExpressionAutoCompleteCommand = new RelayCommand(this.OnExpressionAutoCompleteCommand);
			this.ExpressionCaretIndex = 0;
		}

		private void OnExpressionAutoCompleteCommand(object parameter) {
			if (this.Expression.Equals(this.Suggestion) || String.IsNullOrEmpty(this.Suggestion)) {
				return;
			}
			this._expression = this.Suggestion;
			this.OnPropertyChanged("Expression");
			this.ExpressionCaretIndex = this.Expression.Length;
			this.OnPropertyChanged("ExpressionCaretIndex");
		}

		private static SearchEngine InitialisationSearchEngine() {
			var searchEngine = new SearchEngine();
			searchEngine.RegisterMethod(new ExplorerSearchMethod());
			return searchEngine;
		}



		private SearchEngine SearchEngine {
			get { return this._searchEngine.Value; }
		}

		private void StartSearching() {
			if (String.IsNullOrEmpty(this._expression)) {
				return;
			}
			var suggestions = this.SearchEngine.Search(this.Expression);
			foreach (var suggestion in suggestions) {
				Console.WriteLine(suggestion.Value);
			}
			var best = suggestions.FirstOrDefault();
			this.Suggestion = best != null ? best.Value : string.Empty;
		}
	}
}