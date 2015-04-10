using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Orion.Zeta.Core.SearchMethods {
	public class ExplorerSearchMethod : ISearchMethod {
		private readonly Regex _rgx;

		public ExplorerSearchMethod() {
			this._rgx = new Regex(@"^(\/|[A-Z])(.[A-Za-z0-9\/]*)?");
		}

		public bool IsMatching(string expression) {
			return this._rgx.IsMatch(expression);
		}

		public IEnumerable<IItem> Search(string expression) {
			throw new System.NotImplementedException();
		}
	}
}