using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Orion.Zeta.Controls;
using Orion.Zeta.ViewModels;

namespace Orion.Zeta.Settings.ViewModels {
	public class SettingViewModel : BaseViewModel {
		public ObservableCollection<MenuPanelItem> MenuItems { get; private set; }

		public SettingViewModel() {
			
			this.MenuItems = new ObservableCollection<MenuPanelItem> { new MenuPanelItem {
				Header = "General",
				Icon = Application.Current.FindResource("appbar_settings") as Canvas
			},
			new MenuPanelItem {
				Header = "Style",
				Icon = Application.Current.FindResource("appbar_draw_paintbrush") as Canvas
			}};
		}
	}
}