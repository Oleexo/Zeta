using System;
using System.Collections.Generic;

namespace Orion.Zeta.Core.SearchMethods.ExplorerSearch {
	public class CollectionExpressionPath : List<PossibilityPath> {
		private readonly IComparer<PossibilityPath> comparer;

		public CollectionExpressionPath(IComparer<PossibilityPath> comparer) {
			this.comparer = comparer;
		}

		public void SortAndRank() {
			this.Sort(this.comparer);
			for (var i = 0; i < this.Count; ++i) {
				this[i].Rank = i;
			}
		}
	}

	 internal class SortPathBetterMatchStrategy : IComparer<PossibilityPath> {
		public int Compare(PossibilityPath x, PossibilityPath y) {
			if (x.Path.Length > y.Path.Length)
				return 1;
			if (x.Path.Length < y.Path.Length)
				return -1;
			return String.Compare(x.Path, y.Path, StringComparison.OrdinalIgnoreCase);
		}
	}

	internal class SortPathWindowsStyle : IComparer<PossibilityPath> {
		public int Compare(PossibilityPath x, PossibilityPath y) {
			if (x.Type == PossibilityPath.PathType.Directory && y.Type == PossibilityPath.PathType.File)
				return 1;
			if (x.Type == PossibilityPath.PathType.File&& y.Type == PossibilityPath.PathType.Directory)
				return -1;
			return String.Compare(x.Path, y.Path, StringComparison.OrdinalIgnoreCase);
		}
	}
}