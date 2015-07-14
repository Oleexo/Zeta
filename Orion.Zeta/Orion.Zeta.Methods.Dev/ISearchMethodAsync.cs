using System.Collections.Generic;
using System.Threading.Tasks;
using Orion.Zeta.Methods.Dev.Shared;

namespace Orion.Zeta.Methods.Dev {
	public interface ISearchMethodAsync : ISearchMethod {

		Task InitialisationAsync();

		Task<IEnumerable<IItem>> SearchAsync(string expression);
	}
}