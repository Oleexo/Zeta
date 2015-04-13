using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Orion.Zeta.Core.SearchMethods.ExplorerSearch {
	public class ExplorerSearchMethod : ISearchMethod {
		private readonly Regex _rgx;
		private const string PatternRegex = @"^(\/|[A-Za-z~]\/)(.[.A-Za-z0-9\/\s()-\[\]]*)?$";

		public ExplorerSearchMethod() {
			this._rgx = new Regex(PatternRegex);
		}

		public bool IsMatching(string expression) {
			return this._rgx.IsMatch(expression);
		}

		public IEnumerable<IItem> Search(string expression) {
			// Todo convert expression to win path -> list
			var expressionPath = new ExpressionPath(expression);

			// Todo find possibiities with win Path -> list
			expressionPath.FindPossibilities();

			// todo convert result to expression path -> list
			return expressionPath.GetItems();
		}
	}
}