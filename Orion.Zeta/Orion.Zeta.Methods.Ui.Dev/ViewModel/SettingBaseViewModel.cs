using System.Threading.Tasks;
using Orion.Zeta.Methods.Dev.Setting;

namespace Orion.Zeta.Methods.Ui.Dev.ViewModel {
	public abstract class SettingBaseViewModel<TData> : BaseViewModel where TData : class, new() {
		protected readonly IDataService DataService;
		protected TData _model;
		protected string _defaultConfigurationFilename = "config";
		protected bool IsModelModified { get; private set; }

		protected SettingBaseViewModel(IDataService dataService) {
			this.DataService = dataService;
			this.IsModelModified = false;
		}

		protected SettingBaseViewModel(IDataService dataService, string configurationFilename) {
			this.DataService = dataService;
			this.IsModelModified = false;
			this._defaultConfigurationFilename = configurationFilename;
		}

		protected Task LoadDataSettingAsync() {
			return Task.Run(() => this.LoadDataSetting());
		}

		protected void LoadDataSetting() {
			this._model = this.DataService.Retrieve<TData>(this._defaultConfigurationFilename) ?? new TData();
		}

		protected Task SaveDataSettingAsync() {
			return Task.Run(() => this.SaveDataSetting());
		}

		protected void SaveDataSetting() {
			if (this.IsModelModified)
				this.DataService.Persist(this._defaultConfigurationFilename, this._model);
		}

		protected void ModelModified() {
			this.IsModelModified = true;
		}
	}
}