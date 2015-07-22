using System.Threading.Tasks;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Methods.Ui {
    public abstract class SettingBaseViewModel<TSearchMethod, TData>: BaseViewModel where TData : class, new() where TSearchMethod : class {
        protected readonly IDataService _dataService;
        protected TSearchMethod _searchMethod;
        protected TData _model;

        protected SettingBaseViewModel(IDataService dataService, ISearchMethod searchMethod) {
            this._dataService = dataService;
            this._searchMethod = searchMethod as TSearchMethod;
        }

        protected Task LoadDataSettingAsync() {
            return Task.Run(() => this.LoadDataSetting());
        }

        protected void LoadDataSetting() {
            this._model = this._dataService.Retrieve<TData>("Application Search") ?? new TData();
        }
    }
}