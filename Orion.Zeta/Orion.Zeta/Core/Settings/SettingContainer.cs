using System.Windows.Controls;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Dev.Setting;
using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Core.Settings {
    public class SettingContainer : ISettingContainer {
        private readonly IBaseMethodContainer _methodContainer;
        private readonly ISearchMethodSettingService _searchMethodSettingService;

        public SettingContainer(IBaseMethodContainer methodContainer, ISearchMethodSettingService searchMethodSettingService) {
            this._methodContainer = methodContainer;
            this._searchMethodSettingService = searchMethodSettingService;
        }

        public string Header => this._methodContainer.Name;

        public bool? Enabled { get; set; }

        public UserControl CreateControl() {
            if (!this._methodContainer.HaveSettingControl)
                return null;
            return this._methodContainer.CreateSettingControl(this._searchMethodSettingService);
        }
    }
}