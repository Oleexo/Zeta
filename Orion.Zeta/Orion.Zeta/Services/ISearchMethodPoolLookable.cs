using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Services {
	public interface ISearchMethodPoolLookable {
		bool ContainSearchMethod(IBaseMethodContainer methodContainer);
		bool ContainSearchMethod(string applicationName);
		ISearchMethod GetInstanceOf(IBaseMethodContainer methodContainer);
		ISearchMethod GetInstanceOf(string applicationName);
	}
}