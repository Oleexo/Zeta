using System.Windows.Controls;
using Orion.Zeta.Methods.Dev;

namespace Orion.Zeta.Methods.Ui.Dev {
    public interface IBaseMethodContainer {
        bool HaveSettingControl { get; }

        UserControl CreateSettingControl(IDataService dataService, ISearchMethod method);

        string Name { get; }
    }
}