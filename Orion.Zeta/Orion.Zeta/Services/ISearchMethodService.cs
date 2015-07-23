using Orion.Zeta.Core;

namespace Orion.Zeta.Services {
	public interface ISearchMethodService {
		void RegisterSearchMethods(SettingsService settingsService);

		void ManageMethodsBySetting(SettingsService settingsService);

		void RegisterSettings(SettingsService settingsService);
	}
}