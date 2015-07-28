using System;
using Orion.Zeta.Persistence;

namespace Orion.Zeta.Services {
	public class ApplicationSettingService : DataService, IApplicationSettingService {
		private readonly SettingsService _settingsService;

		public ApplicationSettingService(SettingsService settingsService) : base(null, settingsService.SettingRepository) {
			this._settingsService = settingsService;
		}

		public event EventHandler Closing;

		public void OnClosing() {
			this.Closing?.Invoke(this, EventArgs.Empty);
		}
	}
}