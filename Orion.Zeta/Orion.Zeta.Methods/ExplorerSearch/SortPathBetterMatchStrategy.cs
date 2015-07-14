using System;
using System.Collections.Generic;

namespace Orion.Zeta.Methods.ExplorerSearch {
	internal class SortPathBetterMatchStrategy : IComparer<PossibilityPath> {
		public int Compare(PossibilityPath x, PossibilityPath y) {
			if (x.Path.Length > y.Path.Length)
				return 1;
			if (x.Path.Length < y.Path.Length)
				return -1;
			return String.Compare(x.Path, y.Path, StringComparison.OrdinalIgnoreCase);
		}
	}
}