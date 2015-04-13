using System;
using System.Linq;
using System.Windows.Input;
using Orion.Zeta.Core;
using Orion.Zeta.Core.SearchMethods.ExplorerSearch;

namespace Orion.Zeta.ViewModels {
	public class MainViewModel : BaseViewModel {
		private string _suggestion;
		private readonly Lazy<SearchEngine> _searchEngine = new Lazy<SearchEngine>(InitialisationSearchEngine);
		private string _expression;

		public MainViewModel() {
			this.Expression = string.Empty;
			this.Suggestion = string.Empty;
		}

		private static SearchEngine InitialisationSearchEngine() {
			var searchEngine = new SearchEngine();
			searchEngine.RegisterMethod(new ExplorerSearchMethod());
			return searchEngine;
		}

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