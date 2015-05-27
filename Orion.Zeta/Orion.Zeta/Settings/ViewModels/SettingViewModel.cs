using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using Orion.Zeta.Controls;
using Orion.Zeta.Services;
using Orion.Zeta.ViewModels;

namespace Orion.Zeta.Settings.ViewModels {
    public class SettingViewModel : BaseViewModel {
        private readonly SettingsService _settingsService;
        private UserControl _currentSetting;
        private object _currentSelectedItem;
        private string _currentSettingName;
        private bool _isDisactivable;
        private bool? _enabled;


        public ObservableCollection<MenuPanelItemSetting> MenuItems { get; private set; }

        public UserControl CurrentSetting {
            get { return this._currentSetting; }
            set {
                this._currentSetting = value;
                this.OnPropertyChanged();
            }
        }

        public string CurrentSettingName {
            get { return this._currentSettingName; }
            set {
                this._currentSettingName = value;
                this.OnPropertyChanged();
            }
        }

        public object CurrentSelectedItem {
            get { return this._currentSelectedItem; }
            set {
                this._currentSelectedItem = value;
                this.SetCurrentSetting(value);
            }
        }

        public bool IsDisactivable {
            get { return this._isDisactivable; }
            set {
                this._isDisactivable = value;
                this.OnPropertyChanged();
            }
        }

        public bool? Enabled
        {
            get { return this._enabled; }
            set
            {
                this._enabled = value;
                this.OnPropertyChanged();
            }
        }

        public SettingViewModel(SettingsService settingsService) {
            this._settingsService = settingsService;
            var settingContainers = this._settingsService.GetSettingContainers();
            var globalSettingContainers = this._settingsService.GetGlobalSettingContainers();
            this.MenuItems = new ObservableCollection<MenuPanelItemSetting>();
            foreach (var globalSettingContainer in globalSettingContainers) {
                var menuItem = new MenuPanelItemSetting {
                    Header = globalSettingContainer.Header,
                    Control = globalSettingContainer.CreateControl(),
                    Enabled = globalSettingContainer.Enabled,
                };
                this.MenuItems.Add(menuItem);
            }

            foreach (var settingContainer in settingContainers) {
                var menuItem = new MenuPanelItemSetting {
                    Header = settingContainer.Header,
                    Control = settingContainer.CreateControl(),
                    Enabled = settingContainer.Enabled,
                };
                this.MenuItems.Add(menuItem);
            }
            var item = this.MenuItems.FirstOrDefault();
            this.CurrentSetting = item?.Control;
            this.CurrentSettingName = item?.Header ?? "No setting module";
        }


        private void SetCurrentSetting(object item) {
            var itemPanel = item as MenuPanelItemSetting;
            this.CurrentSetting = itemPanel?.Control;
            this.CurrentSettingName = itemPanel?.Header ?? "Setting";
            this.Enabled = itemPanel?.Enabled;
        }

        public class MenuPanelItemSetting : MenuPanelItem {
            public bool? Enabled { get; set; }
        }
    }
}