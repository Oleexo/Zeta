namespace Orion.Zeta.Services {
	public interface ISearchMethodService {
		void RegisterSearchMethods(SettingsService settingsService);

		void ManageMethodsBySetting(SettingsService settingsService);

		void RegisterSettings(SettingsService settingsService);

		void ToggleMethod(string searchMethodName, SettingsService settingsService, bool value);
	}
}