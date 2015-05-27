using System;
using System.Windows.Controls;
using Orion.Zeta.Persistence;

namespace Orion.Zeta.Core.Settings {
    public interface ISettingContainer {
        Type ControlType { get; set; }

        string Header { get; set; }

        bool? Enabled { get; set; }

        void ApplyChanges();

        bool HaveDefaultData();

        UserControl CreateControl();

        void ReadData(ISettingRepository settingRepository);

        void WriteData(ISettingRepository settingRepository);
    }
}