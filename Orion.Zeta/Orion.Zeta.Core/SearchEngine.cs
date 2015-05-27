using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orion.Zeta.Core.SearchMethods;

namespace Orion.Zeta.Core {
	public class SearchEngine : ISearchEngine {
		private readonly IList<ISearchMethod> _searchMethods;
		private readonly IList<ISearchMethodAsync> _searchMethodsAsync;

		public SearchEngine() {
			this._searchMethods = new List<ISearchMethod>();
			this._searchMethodsAsync = new List<ISearchMethodAsync>();
		}

		public void RegisterMethod(ISearchMethod method) {
			this._searchMethods.Add(method);
			method.Initialisation();
		}

		public void RegisterMethod(ISearchMethodAsync method) {
			this._searchMethodsAsync.Add(method);
			method.InitialisationAsync();
		}

		public async Task<IEnumerable<IItem>> Search(string expression) {
			var items = new List<IItem>();
			IList<Task<IEnumerable<IItem>>> tasks = new List<Task<IEnumerable<IItem>>>();
			
			foreach (var searchMethod in this._searchMethods.Where(sm => sm.IsMatching(expression))) {
				items = searchMethod.Search(expression).Concat(items).ToList();
			}
			foreach (var searchMethodAsync in this._searchMethodsAsync.Where(sma => sma.IsMatching(expression))) {
				tasks.Add(searchMethodAsync.SearchAsync(expression));
			}
			if (!tasks.Any()) 
				return items;
			await Task.WhenAll(tasks);
			foreach (var task in tasks) {
				items = task.Result.Concat(items).ToList();
			}
			return items;
		}

	    public void RefreshCache() {
	        foreach (var searchMethod in this._searchMethods) {
	            searchMethod.RefreshCache();
	        }

	        foreach (var searchMethodAsync in this._searchMethodsAsync) {
	            searchMethodAsync.RefreshCache();
	        }
	    }
	}
}