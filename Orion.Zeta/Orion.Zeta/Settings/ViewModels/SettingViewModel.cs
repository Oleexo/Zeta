using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using Orion.Zeta.Controls;
using Orion.Zeta.Core.Settings;
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
        private MenuPanelItemSetting _currentItemPanel;


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

        public bool? Enabled {
            get { return this._enabled; }
            set {
                this._enabled = value;
                this._currentItemPanel.Enabled = value;
                this.OnPropertyChanged();
            }
        }

        public SettingViewModel(SettingsService settingsService) {
            this._settingsService = settingsService;
            var settingContainers = this._settingsService.GetSettingContainers();
            var globalSettingContainers = this._settingsService.GetGlobalSettingContainers();
            this.MenuItems = new ObservableCollection<MenuPanelItemSetting>();
            foreach (var globalSettingContainer in globalSettingContainers) {
                this.MenuItems.Add(new MenuPanelItemSetting(globalSettingContainer));
            }

            foreach (var settingContainer in settingContainers) {
                this.MenuItems.Add(new MenuPanelItemSetting(settingContainer));
            }
            var item = this.MenuItems.FirstOrDefault();
            this.CurrentSetting = item?.Control;
            this.CurrentSettingName = item?.Header ?? "No setting module";
        }


        private void SetCurrentSetting(object item) {
            this._currentItemPanel = item as MenuPanelItemSetting;
            this.CurrentSetting = this._currentItemPanel?.Control;
            this.CurrentSettingName = this._currentItemPanel?.Header ?? "Setting";
            this.Enabled = this._currentItemPanel?.Enabled;
        }

        public class MenuPanelItemSetting : MenuPanelItem {
            private readonly ISettingContainer _settingContainer;

            public MenuPanelItemSetting(ISettingContainer settingContainer) {
                this._settingContainer = settingContainer;
                this.Header = settingContainer.Header;
                this.Control = settingContainer.CreateControl();

            }

            public bool? Enabled {
                get { return this._settingContainer.Enabled; }
                set { this._settingContainer.Enabled = value; }
            }
        }
    }
}