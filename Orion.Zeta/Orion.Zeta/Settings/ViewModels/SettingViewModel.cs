using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Orion.Zeta.Controls;
using Orion.Zeta.Settings.Views;
using Orion.Zeta.ViewModels;

namespace Orion.Zeta.Settings.ViewModels {
    public class SettingViewModel : BaseViewModel {
        public ObservableCollection<MenuPanelItem> MenuItems { get; private set; }

        public UserControl CurrentSetting {
            get { return this.currentSetting; }
            set {
                this.currentSetting = value;
                this.OnPropertyChanged();
            }
        }

        public object CurrentSelectedItem
        {
            get { return this.currentSelectedItem; }
            set
            {
                this.currentSelectedItem = value; 
                this.SetCurrentSetting(value);
            }
        }

        public SettingViewModel() {
            var panelItem = new MenuPanelItem {
                Header = "General",
                Icon = Application.Current.FindResource("appbar_settings") as Canvas,
                Control = new GeneralView()
            };
            this.MenuItems = new ObservableCollection<MenuPanelItem> { panelItem,
                new MenuPanelItem {
                    Header = "Style",
                    Icon = Application.Current.FindResource("appbar_draw_paintbrush") as Canvas,
                    Control = new StyleView()
                }};
            this.CurrentSetting = panelItem.Control;
        }

        private UserControl currentSetting;
        private object currentSelectedItem;

        private void SetCurrentSetting(object item) {
            var itemPanel = item as MenuPanelItem;
            this.CurrentSetting = itemPanel?.Control;
        }
    }
}