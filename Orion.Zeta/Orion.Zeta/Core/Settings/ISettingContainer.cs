using System;
using Orion.Zeta.Controls;
using Orion.Zeta.Persistence;

namespace Orion.Zeta.Core.Settings {
    public interface ISettingContainer {
        Type ControlType { get; set; }

        string Header { get; set; }

        MenuPanelItem ToMenuPanelItem();

        void ApplyChanges();

        bool HaveDefaultData();

        void ReadData(ISettingRepository settingRepository);

        void WriteData(ISettingRepository settingRepository);
    }
}