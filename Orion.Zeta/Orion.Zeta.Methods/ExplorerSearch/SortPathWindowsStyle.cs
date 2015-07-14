using System;
using System.Collections.Generic;

namespace Orion.Zeta.Methods.ExplorerSearch {
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