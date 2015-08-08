using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Dev.Setting;
using Orion.Zeta.Methods.ExplorerSearch;
using Orion.Zeta.Methods.Ui.Dev;
using Orion.Zeta.Methods.Ui.Dev.MethodContainers;

namespace Orion.Zeta.Methods.Ui.ExplorerSearch {
    [Export(typeof(IMethodAsyncContainer))]
    public class ExplorerSearchContainer : IMethodAsyncContainer {

        public bool HaveSettingControl => false;

        public UserControl CreateSettingControl(ISearchMethodSettingService searchMethodSettingService) {
            throw new NotImplementedException();
        }

        public string Name => "Explorer";

		ISearchMethod IMethodContainer.GetNewInstanceOfSearchMethod(IDataService dataService) {
		    return this.GetNewInstanceOfSearchMethod(dataService);
	    }

	    public ISearchMethodAsync GetNewInstanceOfSearchMethod(IDataService dataService) {
		    return new ExplorerSearchMethod();
	    }
    }
}