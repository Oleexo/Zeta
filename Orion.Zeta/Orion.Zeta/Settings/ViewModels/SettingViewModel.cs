using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using Orion.Zeta.Controls;
using Orion.Zeta.Services;
using Orion.Zeta.ViewModels;

namespace Orion.Zeta.Settings.ViewModels {
    public class SettingViewModel : BaseViewModel {
        private readonly SettingsService settingsService;
        private UserControl currentSetting;
        private object currentSelectedItem;


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

        public SettingViewModel(SettingsService settingsService) {
            this.settingsService = settingsService;
            var settingContainers = this.settingsService.GetSettingContainers();
            this.MenuItems = new ObservableCollection<MenuPanelItem>();
            foreach (var settingContainer in settingContainers) {
                this.MenuItems.Add(settingContainer.ToMenuPanelItem());
            }
            this.CurrentSetting = this.MenuItems.FirstOrDefault()?.Control;
        }


        private void SetCurrentSetting(object item) {
            var itemPanel = item as MenuPanelItem;
            this.CurrentSetting = itemPanel?.Control;
        }
    }
}