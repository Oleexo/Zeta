using System;
using System.Windows.Controls;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Persistence;

namespace Orion.Zeta.Core.Settings {
    public class GeneralSettingContainer<T> : IGeneralSettingContainer where T : class, new() {
        public T Data { get; set; }

        public Type ControlType { get; set; }

        public string Header { get; set; }

        public bool? Enabled { get; set; }

        private readonly IModifiableSettings _modifiableSettings;

        public GeneralSettingContainer(string header, Type type, IModifiableSettings modifiableSettings) {
            this._modifiableSettings = modifiableSettings;
            this.Header = header;
            this.ControlType = type;
            this.Data = default(T);
        }

        public GeneralSettingContainer(string header, Type type, IModifiableSettings modifiableSettings, T data) {
            this._modifiableSettings = modifiableSettings;
            this.Header = header;
            this.ControlType = type;
            this.Data = data;
        }

        public void ApplyChanges() {
            this._modifiableSettings?.ApplyChanges(this.Data);
        }

        public bool HaveDefaultData() {
            return this.Data != null;
        }

        public void ReadData(ISettingRepository settingRepository) {
            var dataBox = settingRepository.Find<DataBox<T>>(this.Header) as DataBox<T>;
            if (dataBox == null && this.Data == null) {
                this.Data = new T();
                this.Enabled = true;
            }
            else if (dataBox != null) {
                this.Data = dataBox.Data;
                this.Enabled = dataBox.Enabled;
            }
            if (!this.Enabled.HasValue) {
                this.Enabled = true;
            }
        }

        public void WriteData(ISettingRepository settingRepository) {
            settingRepository.Persite(this.Header, new DataBox<T> {
                Enabled = this.Enabled,
                Data = this.Data
            });
        }

        public UserControl CreateControl() {
            return Activator.CreateInstance(this.ControlType, this.Data) as UserControl;
        }

        public class DataBox<TData> {
            public bool? Enabled { get; set; }

            public TData Data { get; set; }
        }
    }
}