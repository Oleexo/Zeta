using System.Collections.Generic;
using Orion.Zeta.Methods.Dev.Shared;

namespace Orion.Zeta.Methods.Dev {
	public interface ISearchMethod {
		bool IsMatching(string expression);

		void Initialisation();

		IEnumerable<IItem> Search(string expression);

	    void RefreshCache();
	}
}