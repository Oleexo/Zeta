﻿using System.Windows.Controls;
using Orion.Zeta.Methods.Dev.Setting;

namespace Orion.Zeta.Methods.Ui.Dev {
    public interface IBaseMethodContainer {
        bool HaveSettingControl { get; }

        UserControl CreateSettingControl(ISearchMethodSettingService searchMethodSettingService);

        string Name { get; }
    }
}