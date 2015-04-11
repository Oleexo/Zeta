using System.Collections.Generic;
using System.Linq;
using Orion.Zeta.Core.SearchMethods;

namespace Orion.Zeta.Core {
	public class SearchEngine : ISearchEngine {
		private List<ISearchMethod> _searchMethods;

		public SearchEngine() {
			this._searchMethods = new List<ISearchMethod>();
		}

		public void RegisterMethod(ISearchMethod method) {
			this._searchMethods.Add(method);
		}

		public IEnumerable<IItem> Search(string expression) {
			var items = new List<IItem>();
			foreach (var searchMethod in this._searchMethods) {
				if (searchMethod.IsMatching(expression))
					items = searchMethod.Search(expression).Concat(items).ToList();
			}
			return items;
		}
	}
}