using System.ComponentModel.Composition;
using System.Windows.Controls;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.ExplorerSearch;
using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Methods.Ui.ExplorerSearch {
    [Export(typeof(IMethodAsyncContainer))]
    public class ExplorerSearchContainer : IMethodAsyncContainer {

        public bool HaveSettingControl => false;

        public UserControl CreateSettingControl(IDataService dataService) {
            throw new System.NotImplementedException();
        }

        public ISearchMethodAsync SearchMethod => new ExplorerSearchMethod();
        public string Name => "Explorer";
    }
}