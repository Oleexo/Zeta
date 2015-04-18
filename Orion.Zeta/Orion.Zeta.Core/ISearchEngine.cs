using System.Collections.Generic;
using System.Threading.Tasks;
using Orion.Zeta.Core.SearchMethods;

namespace Orion.Zeta.Core {
	public interface ISearchEngine {
		void RegisterMethod(ISearchMethod method);

		void RegisterMethod(ISearchMethodAsync method);

		Task<IEnumerable<IItem>> Search(string expression);
	}
}