using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Dev.Shared;
using Orion.Zeta.Methods.Dev.Shared.Implementations;

namespace Orion.Zeta.Methods.ExplorerSearch {
	public class ExplorerSearchMethod : ISearchMethodAsync {
		private readonly Regex _rgx;
		private readonly FileSystemSearch _fileSystemSearch;
		private Task _initialisationTask;
		private const string PatternRegex = @"^(\/|[A-Za-z~]\/)(.[.\w+\/\s()-\[\]]*)?$";

		public ExplorerSearchMethod() {
			this._rgx = new Regex(PatternRegex);
			this._fileSystemSearch = new FileSystemSearch();
		}

		public bool IsMatching(string expression) {
			return this._rgx.IsMatch(expression);
		}

		public void Initialisation() {
		}

		public IEnumerable<IItem> Search(string expression) {
			var expressionPath = new ExpressionPath(expression, this._fileSystemSearch);

			var possibilities = expressionPath.FindPossibilities();

			return possibilities.Select(p => p.ToItem());
		}

	    public void RefreshCache() {}

	    public Task InitialisationAsync() {
			this._initialisationTask = Task.Run(() => {
				this.Initialisation();
			});
			return this._initialisationTask;
		}

		public async Task<IEnumerable<IItem>> SearchAsync(string expression) {
			if (this._initialisationTask != null && !this._initialisationTask.IsCompleted) {
				await this._initialisationTask;
			}
			return await Task.Run(() => this.Search(expression));
		}
	}
}