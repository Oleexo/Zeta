using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orion.Zeta.Core.SearchMethods {
	public interface ISearchMethodAsync : ISearchMethod {

		Task InitialisationAsync();

		Task<IEnumerable<IItem>> SearchAsync(string expression);
	}
}