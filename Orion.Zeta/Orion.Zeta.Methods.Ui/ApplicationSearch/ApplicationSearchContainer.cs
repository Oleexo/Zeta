using System.ComponentModel.Composition;
using System.Windows.Controls;
using Orion.Zeta.Methods.ApplicationSearch;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Methods.Ui.ApplicationSearch {
    [Export(typeof(IMethodAsyncContainer))]
    public class ApplicationSearchContainer : IMethodAsyncContainer {
        public bool HaveSettingControl => true;

        public UserControl CreateSettingControl(IDataService dataService) {
            return new ApplicationSearchView(dataService);
        }

        public ISearchMethodAsync SearchMethod => new ApplicationSearchMethod();

        public string Name => "Applications";
    }
}