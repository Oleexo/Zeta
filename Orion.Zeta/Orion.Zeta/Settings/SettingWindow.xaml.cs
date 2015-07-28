using MahApps.Metro.Controls;
using Orion.Zeta.Services;
using Orion.Zeta.Settings.ViewModels;

namespace Orion.Zeta.Settings {
	public partial class SettingWindow : MetroWindow {
		public SettingWindow(ISettingsService settingsService, ISearchMethodService searchMethodService) {
			this.InitializeComponent();
			this.DataContext = new SettingViewModel(settingsService, searchMethodService);
		}
	}
}
