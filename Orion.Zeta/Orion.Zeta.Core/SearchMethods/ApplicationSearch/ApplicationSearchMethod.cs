using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Orion.Zeta.Core.SearchMethods.Shared;

namespace Orion.Zeta.Core.SearchMethods.ApplicationSearch {
	public class ApplicationSearchMethod : ISearchMethod {
		private readonly List<ApplicationsContainer> _applications;
		private readonly IFileSystemSearch _fileSystemSearch;
		private readonly Regex _rgx;

		public ApplicationSearchMethod() {
			this._fileSystemSearch = new FileSystemSearch();
			this._applications = new List<ApplicationsContainer>();
			this._rgx = new Regex("^(?!.*/|:|@).*$");
		}

		public bool IsMatching(string expression) {
			return this._rgx.IsMatch(expression);
		}

		public IEnumerable<IItem> Search(string expression) {
			var items = new List<IItem>();
			foreach (var container in this._applications) {
				items = container.Search(expression).Concat(items).ToList();
			}
			return items;
		}

		public void RegisterPath(string path, IEnumerable<string> patterns) {
			this._applications.Add(new ApplicationsContainer(path, patterns, this._fileSystemSearch));
		}

		public bool IsRegistered(string path) {
			return this._applications.Any(a => a.Path.Equals(path));
		}
	}
}