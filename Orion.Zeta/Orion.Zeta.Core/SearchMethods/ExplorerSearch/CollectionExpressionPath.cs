using System.Collections.Generic;

namespace Orion.Zeta.Core.SearchMethods.ExplorerSearch {
	public class CollectionExpressionPath : List<PossibilityPath> {
		public void SortAndRank() {
			this.Sort();
			for (var i = 0; i < this.Count; ++i) {
				this[i].Rank = i;
			}
		}
	}
}