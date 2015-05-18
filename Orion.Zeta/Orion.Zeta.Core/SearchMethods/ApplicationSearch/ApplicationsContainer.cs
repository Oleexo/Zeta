using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Orion.Zeta.Core.SearchMethods.Shared;

namespace Orion.Zeta.Core.SearchMethods.ApplicationSearch {
	public class ApplicationsContainer {
		private readonly string _path;
		private readonly IEnumerable<string> _patterns;
		private readonly IFileSystemSearch _fileSystemSearch;
		private IList<Item> _items;

		public ApplicationsContainer(string path, IEnumerable<string> patterns, IFileSystemSearch fileSystemSearch, bool buildCache = true) {
			if (!fileSystemSearch.DirectoryExists(path)) {
				throw new DirectoryNotFoundException(path);
			}
			if (fileSystemSearch == null) {
				throw new ArgumentNullException();
			}
			this._path = path;
			this._patterns = patterns;
			this._fileSystemSearch = fileSystemSearch;
			if (buildCache)
				this.BuildCache();
		}

		public void BuildCache() {
			var matchedFiles = this.GetMatchedFiles();
			this._items = matchedFiles.Select(mf => new Item {
				Value = mf,
				DisplayName = this._fileSystemSearch.GetFilename(mf),
				Icon = this._fileSystemSearch.GetIcon(mf),
				Execute = new Execute {
					Program = mf
				}
			}).ToList();
		}

		private IEnumerable<IItem> RankList(IEnumerable<Item> matchedFiles, string expression) {
			var ranker = new RankApplicationStrategy();
			var rankedList = matchedFiles.Select(m => m.Clone());
			foreach (var item in rankedList) {
				item.Rank = ranker.GetRank(item, expression);
			}
			return rankedList;
		}

		public void ClearCache() {
			this._items.Clear();
		}

		public IEnumerable<IItem> Search(string expression) {
			if (String.IsNullOrEmpty(expression))
				return new List<IItem>();
			var regex = new Regex(this.ConvertWildcardToRegex(this.ConvertExpressionToWildCard(expression)), RegexOptions.IgnoreCase);
			return this.RankList(this._items.Where(mf => regex.IsMatch(mf.DisplayName)), expression);
		}

		private string ConvertExpressionToWildCard(string expression) {
			var chars = new char[expression.Length * 2 + 1];
			chars[0] = '*';
			var position = 1;
			foreach (var @char in expression) {
				chars[position] = @char;
				++position;
				chars[position] = '*';
				++position;
			}
			return new string(chars);
		}

		private IEnumerable<string> GetMatchedFiles() {
			var files = this._fileSystemSearch.GetFiles(this._path, "*", SearchOption.AllDirectories);

			var regexs = this._patterns.Select(pattern => new Regex(this.ConvertWildcardToRegex(pattern))).ToList();
			return files.Where(f => regexs.Any(r => r.IsMatch(f)));
		}

		private string ConvertWildcardToRegex(string wildcard) {
			return "^" + Regex.Escape(wildcard).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
		}

		public string Path {
			get { return this._path; }
		}
	}

	public class RankApplicationStrategy {
		public int GetRank(Item item, string expression) {
			if (item.DisplayName.Equals(expression, StringComparison.OrdinalIgnoreCase)) {
				return 0;
			}
			return this.RankBasedOnUpperCase(expression, item.DisplayName);
		}

		private int RankBasedOnUpperCase(string expression, string itemName) {
			var numberCharMatch = 0;
			var numberUpperCaseMatch = 0;
			var posItemName = 0;
			var posExpression = 0;
			while (posExpression < expression.Length && posItemName < itemName.Length) {
				if (expression[posExpression] == itemName[posItemName]) {
					++numberCharMatch;
					if (Char.IsUpper(expression[posExpression]))
						++numberUpperCaseMatch;
					++posExpression;
					++posItemName;
				}
				++posItemName;
			}
			var rank = (itemName.Length - numberCharMatch) * 5 - numberUpperCaseMatch * 2;
			return rank;
		}
	}
}