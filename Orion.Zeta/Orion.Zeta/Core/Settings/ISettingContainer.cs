using System.Windows.Controls;

namespace Orion.Zeta.Core.Settings {
    public interface ISettingContainer {
        string Header { get; }

        bool? Enabled { get; set; }

        UserControl CreateControl();
    }
}