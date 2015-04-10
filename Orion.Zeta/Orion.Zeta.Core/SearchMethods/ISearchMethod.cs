using System.Collections.Generic;

namespace Orion.Zeta.Core.SearchMethods {
	public interface ISearchMethod {
		bool IsMatching(string expression);

		IEnumerable<IItem> Search(string expression);
	}
}