using System;

namespace Orion.Zeta.Services {
	public class ApplicationSettingService : DataService, IApplicationSettingService {
		private readonly ISettingsService _settingsService;

		public ApplicationSettingService(ISettingsService settingsService) : base(null, settingsService.SettingRepository) {
			this._settingsService = settingsService;
		}

		public event EventHandler Closing;

		public void OnClosing() {
			this.Closing?.Invoke(this, EventArgs.Empty);
		}
	}
}