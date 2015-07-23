using System.ComponentModel.Composition;
using System.Windows.Controls;
using Orion.Zeta.Methods.ApplicationSearch;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Dev.Setting;
using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Methods.Ui.ApplicationSearch {
    [Export(typeof(IMethodAsyncContainer))]
    public class ApplicationSearchContainer : IMethodAsyncContainer {
        public bool HaveSettingControl => true;

        public UserControl CreateSettingControl(ISearchMethodSettingService searchMethodSettingService) {
            return new ApplicationSearchView(searchMethodSettingService);
        }

        public string Name => "Applications";
	    ISearchMethod IMethodContainer.GetNewInstanceOfSearchMethod(IDataService dataService) {
		    return this.GetNewInstanceOfSearchMethod(dataService);
	    }

	    public ISearchMethodAsync GetNewInstanceOfSearchMethod(IDataService dataService) {
		    return new ApplicationSearchMethod();
		}
    }
}