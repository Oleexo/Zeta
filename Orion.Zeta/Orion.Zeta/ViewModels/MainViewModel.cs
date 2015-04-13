using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.TeamFoundation.MVVM;
using Orion.Zeta.Core;
using Orion.Zeta.Core.SearchMethods.ExplorerSearch;

namespace Orion.Zeta.ViewModels {
	public class MainViewModel : BaseViewModel {
		public ICommand ExpressionAutoCompleteCommand { get; set; }

		public event EventHandler OnAutoComplete;

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

		public ObservableCollection<IItem> Suggestions { get; set; } 

		private string _suggestion;
		private readonly Lazy<SearchEngine> _searchEngine = new Lazy<SearchEngine>(InitialisationSearchEngine);
		private string _expression;

		public MainViewModel() {
			this.ExpressionAutoCompleteCommand = new RelayCommand(this.OnExpressionAutoCompleteCommand);
			this.Suggestions = new ObservableCollection<IItem>();
			this.Suggestion = string.Empty;
			this.Expression = string.Empty;
		}

		private void OnExpressionAutoCompleteCommand() {
			if (this.Expression.Equals(this.Suggestion) || String.IsNullOrEmpty(this.Suggestion)) {
				return;
			}
			this._expression = this.Suggestion;
			this.OnPropertyChanged("Expression");
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

		private void StartSearching() {
			if (String.IsNullOrEmpty(this._expression)) {
				return;
			}
			var suggestions = this.SearchEngine.Search(this.Expression);
			this.Suggestions.Clear();
			foreach (var suggestion in suggestions) {
				this.Suggestions.Add(suggestion);
				Console.WriteLine(suggestion.Value);
			}
			var best = suggestions.FirstOrDefault();
			this.Suggestion = best != null ? best.Value : string.Empty;
		}
	}
}