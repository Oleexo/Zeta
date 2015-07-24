using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Services {
	public interface ISearchMethodPoolModifiable {
		void Add(IMethodContainer methodContainer, SettingsService settingsService);

		void Remove(IBaseMethodContainer methodContainer);
	}
}