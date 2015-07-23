using System.Threading.Tasks;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Dev.Setting;
using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Methods.Ui {
    public abstract class SettingBaseViewModel<TSearchMethod, TData>: BaseViewModel where TData : class, new() where TSearchMethod : class {
        protected readonly ISearchMethodSettingService _searchMethodSettingService;
        protected TData _model;

        protected SettingBaseViewModel(ISearchMethodSettingService searchMethodSettingService) {
            this._searchMethodSettingService = searchMethodSettingService;
        }

        protected Task LoadDataSettingAsync() {
            return Task.Run(() => this.LoadDataSetting());
        }

        protected void LoadDataSetting() {
            this._model = this._searchMethodSettingService.Retrieve<TData>("Application Search") ?? new TData();
        }
    }
}