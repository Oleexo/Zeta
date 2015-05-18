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
}