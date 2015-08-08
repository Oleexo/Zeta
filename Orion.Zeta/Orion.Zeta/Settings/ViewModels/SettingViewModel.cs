using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Orion.Zeta.Controls;
using Orion.Zeta.Core.Settings;
using Orion.Zeta.Methods.Ui.Dev;
using Orion.Zeta.Methods.Ui.Dev.Tools.MVVM;
using Orion.Zeta.Services;
using BaseViewModel = Orion.Zeta.ViewModels.BaseViewModel;

namespace Orion.Zeta.Settings.ViewModels {
    public class SettingViewModel : BaseViewModel {
        private readonly ISettingsService _settingsService;
	    private readonly ISearchMethodService _searchMethodService;
	    private UserControl _currentSetting;
        private object _currentSelectedItem;
        private string _currentSettingName;
        private bool _isDisactivable;
        private bool? _enabled;
        private MenuPanelItemSetting _currentItemPanel;

        public ObservableCollection<MenuPanelItemSetting> MenuItems { get; }

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
                this._settingsService.ToggleMethod(this._currentItemPanel.Header, value);
				if (value.HasValue)
					this._searchMethodService.ToggleMethod(this._currentItemPanel.Header, this._settingsService, value.Value);
                this._currentItemPanel.Enabled = value;
                this.OnPropertyChanged();
            }
        }

		public ICommand WindowClosingCommand { get; private set; }

		public SettingViewModel(ISettingsService settingsService, ISearchMethodService searchMethodService) {
            this._settingsService = settingsService;
	        this._searchMethodService = searchMethodService;
			this.WindowClosingCommand = new RelayCommand(this.OnWindowClosingCommand);
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
			this.SetCurrentSetting(item);
        }

	    private void OnWindowClosingCommand() {
		    this._currentItemPanel?.OnCloseControl();
	    }


	    private void SetCurrentSetting(object item) {
			this._currentItemPanel?.OnCloseControl();
            this._currentItemPanel = item as MenuPanelItemSetting;
			if (item == null) {
				this.CurrentSettingName = "No setting selected";
				return;
			}
			this.CurrentSetting = this._currentItemPanel?.Control;
            this.CurrentSettingName = this._currentItemPanel?.Header ?? "Setting";
            this.Enabled = this._currentItemPanel?.Enabled;
        }

		#region MenuPanelCustom
		public sealed class MenuPanelItemSetting : MenuPanelItem {
            private readonly ISettingContainer _settingContainer;

	        public override UserControl Control
	        {
		        get { return this._settingContainer.CreateControl(); }
				set { }
	        }

	        public MenuPanelItemSetting(ISettingContainer settingContainer) {
                this._settingContainer = settingContainer;
                this.Header = settingContainer.Header;
            }

            public bool? Enabled {
                get { return this._settingContainer.Enabled; }
                set { this._settingContainer.Enabled = value; }
            }

	        public void OnCloseControl() {
		        this._settingContainer.OnCloseControl();
	        }
        }
		#endregion
	}
}