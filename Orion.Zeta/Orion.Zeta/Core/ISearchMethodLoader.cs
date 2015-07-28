using System.Collections.Generic;
using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Core {
	public interface ISearchMethodLoader {
		void Load(ICollection<IMethodContainer> searchMethods, ICollection<IMethodAsyncContainer> searchMethodsAsync);
	}
}