using System.Collections.Generic;
using Orion.Zeta.Core.SearchMethods;

namespace Orion.Zeta.Core {
	public interface ISearchEngine {
		void RegisterMethod(ISearchMethod method);
		IEnumerable<IItem> Search(string expression);
	}
}