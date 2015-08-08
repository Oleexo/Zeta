using Orion.Zeta.Methods.Ui.Dev;
using Orion.Zeta.Methods.Ui.Dev.MethodContainers;

namespace Orion.Zeta.Services {
	public interface ISearchMethodPoolModifiable {
		void Add(IMethodContainer methodContainer, ISettingsService settingsService);

		void Remove(IBaseMethodContainer methodContainer);
	}
}