using Orion.Zeta.Methods.Dev.Setting;

namespace Orion.Zeta.Methods.Ui.Dev.ViewModel {
	public abstract class SearchMethodSettingBaseViewModel<TSearchMethod, TData> : SettingBaseViewModel<TData> where TData : class, new() where TSearchMethod : class {
		protected readonly ISearchMethodSettingService SearchMethodSettingService;

		protected TSearchMethod SearchMethod {
			get {
				if (this.SearchMethodSettingService.IsEnabled()) {
					return this.SearchMethodSettingService.GetInstanceOfSearchMethod() as TSearchMethod;
				}
				return null;
			}
		}

		protected SearchMethodSettingBaseViewModel(ISearchMethodSettingService searchMethodSettingService) : base(searchMethodSettingService) {
			this.SearchMethodSettingService = searchMethodSettingService;
		}
	}
}