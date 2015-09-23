using System.Collections.Generic;
using System.Threading.Tasks;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Dev.Shared;

namespace Orion.Zeta.Core {
	public interface ISearchEngine {
		void RegisterMethod(ISearchMethod method);
		void RegisterMethod(ISearchMethodAsync method);
		Task<IEnumerable<IItem>> Search(string expression);
	    void UnRegister(ISearchMethod searchMethod);
		void RefreshCache();
		int? AutoRefreshCache { get; set; }
	}
}