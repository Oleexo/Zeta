using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Services {
	public interface ISearchMethodPoolModifiable {
		void Add(IMethodContainer methodContainer, ISettingsService settingsService);

		void Remove(IBaseMethodContainer methodContainer);
	}
}