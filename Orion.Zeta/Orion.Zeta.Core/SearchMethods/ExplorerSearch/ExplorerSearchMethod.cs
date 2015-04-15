using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Orion.Zeta.Core.SearchMethods.Shared;

namespace Orion.Zeta.Core.SearchMethods.ExplorerSearch {
	public class ExplorerSearchMethod : ISearchMethod {
		private readonly Regex _rgx;
		private FileSystemSearch _fileSystemSearch;
		private const string PatternRegex = @"^(\/|[A-Za-z~]\/)(.[.A-Za-z0-9\/\s()-\[\]]*)?$";

		public ExplorerSearchMethod() {
			this._rgx = new Regex(PatternRegex);
			this._fileSystemSearch = new FileSystemSearch();
		}

		public bool IsMatching(string expression) {
			return this._rgx.IsMatch(expression);
		}

		public IEnumerable<IItem> Search(string expression) {
			var expressionPath = new ExpressionPath(expression, this._fileSystemSearch);

			var possibilities = expressionPath.FindPossibilities();

			return possibilities.Select(p => p.ToItem());
		}
	}
}