namespace Orion.Zeta.Methods.Dev.Setting {
    public interface ISearchMethodSettingService : IDataService {
	    bool IsEnabled();

	    ISearchMethod GetInstanceOfSearchMethod();
    }
}