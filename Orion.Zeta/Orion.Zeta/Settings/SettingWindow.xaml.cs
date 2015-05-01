using System.Collections.Generic;
using MahApps.Metro.Controls;
using Orion.Zeta.Controls;

namespace Orion.Zeta.Settings {
	public partial class SettingWindow : MetroWindow {
		public SettingWindow() {
			this.InitializeComponent();
			this.PanelMenu.ItemsSource = new List<MenuPanelItem> { new MenuPanelItem {
				Header = "Title",
				Icon = null
			} };
		}
	}
}
