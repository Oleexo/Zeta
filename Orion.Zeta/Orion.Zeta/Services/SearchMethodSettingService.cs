using System;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Dev.Setting;

namespace Orion.Zeta.Services {
    public class SearchMethodSettingService : DataService, ISearchMethodSettingService {
        private readonly string _applicationName;
	    private readonly SettingsService _settingsService;
	    private readonly ISearchMethodPoolLookable _searchMethodPoolLookable;

	    public SearchMethodSettingService(string applicationName, SettingsService settingRepository, ISearchMethodPoolLookable searchMethodPoolLookable) : base(applicationName, settingRepository.SettingRepository) {
		    this._applicationName = applicationName;
		    this._settingsService = settingRepository;
		    this._searchMethodPoolLookable = searchMethodPoolLookable;
	    }

	    public bool IsEnabled() {
		    return this._settingsService.IsEnabled(this._applicationName);
	    }

	    public ISearchMethod GetInstanceOfSearchMethod() {
		    return this._searchMethodPoolLookable.GetInstanceOf(this._applicationName);
	    }

	    public event EventHandler Closing;

	    public void OnClosing() {
		    this.Closing?.Invoke(this, EventArgs.Empty);
	    }
    }
}