using System.Threading.Tasks;
using Orion.Zeta.Methods.Dev.Setting;
using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Methods.Ui {
    public abstract class SettingBaseViewModel<TSearchMethod, TData>: BaseViewModel where TData : class, new() where TSearchMethod : class {
        protected readonly ISearchMethodSettingService SearchMethodSettingService;
        protected TData _model;
	    protected string _defaultConfigurationFilename = "config";

	    protected TSearchMethod SearchMethod
	    {
		    get
		    {
			    if (this.SearchMethodSettingService.IsEnabled()) {
				    return this.SearchMethodSettingService.GetInstanceOfSearchMethod() as TSearchMethod;
			    }
			    return null;
		    }
	    }

        protected SettingBaseViewModel(ISearchMethodSettingService searchMethodSettingService) {
            this.SearchMethodSettingService = searchMethodSettingService;
        }

        protected Task LoadDataSettingAsync() {
            return Task.Run(() => this.LoadDataSetting());
        }

        protected void LoadDataSetting() {
            this._model = this.SearchMethodSettingService.Retrieve<TData>(this._defaultConfigurationFilename) ?? new TData();
        }
    }
}