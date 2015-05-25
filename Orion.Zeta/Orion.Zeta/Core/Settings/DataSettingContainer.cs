using System;
using System.Windows.Controls;
using Orion.Zeta.Controls;
using Orion.Zeta.Core.SearchMethods;
using Orion.Zeta.Persistence;

namespace Orion.Zeta.Core.Settings {
    public class DataSettingContainer<T> : ISettingContainer where T : class, new() {
        public T Data { get; set; }

        public Type ControlType { get; set; }

        public string Header { get; set; }

        private readonly IModifiableSettings _modifiableSettings;

        public DataSettingContainer(string header, Type type, IModifiableSettings modifiableSettings) {
            this._modifiableSettings = modifiableSettings;
            this.Header = header;
            this.ControlType = type;
            this.Data = default(T);
        }

        public DataSettingContainer(string header, Type type, IModifiableSettings modifiableSettings, T data) {
            this._modifiableSettings = modifiableSettings;
            this.Header = header;
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

        public bool HaveDefaultData() {
            return this.Data != null;
        }

        public void ReadData(ISettingRepository settingRepository) {
            this.Data = settingRepository.Find<T>(this.Header) as T ?? new T();
        }

        public void WriteData(ISettingRepository settingRepository) {
            settingRepository.Persite(this.Header, this.Data);
        }

        private UserControl CreateControl() {
            return Activator.CreateInstance(this.ControlType, this.Data) as UserControl;
        }
    }
}