using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Orion.Zeta.Controls;
using Orion.Zeta.Core.SearchMethods;

namespace Orion.Zeta.Services {
    public class SettingsService {
        private List<ISettingContainer> _settingContainers;

        public SettingsService() {
            this._settingContainers = new List<ISettingContainer>();
        }

        public IEnumerable<ISettingContainer> GetSettingContainers() {
            return this._settingContainers;
        }

        public void RegisterGlobal(ISettingContainer settingContainer) {
            this._settingContainers.Add(settingContainer);
        }

        public void Register(ISettingContainer settingContainer) {
            this._settingContainers.Add(settingContainer);
        }

        public void ApplyChanges() {
            foreach (var settingContainer in this._settingContainers) {
                settingContainer.ApplyChanges();
            }
        }
    }

    public class DataSettingContainer : ISettingContainer {
        public object Data { get; set; }

        public Type ControlType { get; set; }

        public string Header { get; set; }

        private readonly IModifiableSettings _modifiableSettings;

        public DataSettingContainer(string general, Type type, IModifiableSettings modifiableSettings) {
            this._modifiableSettings = modifiableSettings;
            this.Header = general;
            this.ControlType = type;
            this.Data = null;
        }

        public DataSettingContainer(string general, Type type, IModifiableSettings modifiableSettings, object data) {
            this._modifiableSettings = modifiableSettings;
            this.Header = general;
            this.ControlType = type;
            this.Data = data;
        }

        public MenuPanelItem ToMenuPanelItem() {
            return new MenuPanelItem {
                Header = this.Header,
                Control = this.CreateControl()
            };
        }

        public void ApplyChanges() {
            this._modifiableSettings?.ApplyChanges(this.Data);
        }

        private UserControl CreateControl() {
            return Activator.CreateInstance(this.ControlType, this.Data) as UserControl;
        }
    }

    public interface ISettingContainer {
        Type ControlType { get; set; }

        string Header { get; set; }

        MenuPanelItem ToMenuPanelItem();

        object Data { get; }

        void ApplyChanges();
    }
}