using System.Windows.Controls;
using Orion.Zeta.Methods.Dev;

namespace Orion.Zeta.Methods.Ui.Dev {
    public interface IMethodContainer {
        // UI Bool
        bool HaveSettingControl { get; }

        // UI
        UserControl CreateSettingControl(IDataService dataService);

        // Search Method
        ISearchMethod SearchMethod { get; }

        string Name { get; }
    }
}