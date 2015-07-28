using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Services {
	public interface ISearchMethodService {
		/// <summary>
		/// Enable method in engine at initialisation
		/// </summary>
		/// <param name="settingsService"></param>
		void RegisterSearchMethods(ISettingsService settingsService);

		/// <summary>
		/// Enable or disable method
		/// </summary>
		/// <param name="settingsService"></param>
		void ManageMethodsBySetting(ISettingsService settingsService);

		void ToggleMethod(IMethodContainer searchMethod, ISettingsService settingsService);
		void ToggleMethod(string searchMethodName, ISettingsService settingsService, bool value);

		/// <summary>
		/// Register method in setting panel
		/// </summary>
		/// <param name="settingsService"></param>
		void RegisterSettings(ISettingsService settingsService);
	}
}