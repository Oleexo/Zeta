using System.Windows.Controls;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Core.Settings {
    public class SettingContainer : ISettingContainer {
        private readonly IBaseMethodContainer _searchMethod;
        private readonly IDataService _dataService;
        private readonly ISearchMethod _method;

        public SettingContainer(IBaseMethodContainer searchMethod, IDataService dataService, ISearchMethod method) {
            this._searchMethod = searchMethod;
            this._dataService = dataService;
            this._method = method;
        }

        public string Header => this._searchMethod.Name;

        public bool? Enabled { get; set; }

        public UserControl CreateControl() {
            if (!this._searchMethod.HaveSettingControl)
                return null;
            return this._searchMethod.CreateSettingControl(this._dataService, this._method);
        }
    }
}