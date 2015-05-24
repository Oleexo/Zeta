using MahApps.Metro.Controls;
using Orion.Zeta.Settings.ViewModels;

namespace Orion.Zeta.Settings {
	public partial class SettingWindow : MetroWindow {
		public SettingWindow() {
			this.InitializeComponent();
			this.DataContext = new SettingViewModel();
		}
	}
}
