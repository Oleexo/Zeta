using Orion.Zeta.Persistence;

namespace Orion.Zeta.Core.Settings {
    public interface IGeneralSettingContainer : ISettingContainer {
        void ReadData(ISettingRepository settingRepository);

        void WriteData(ISettingRepository settingRepository);

        bool HaveDefaultData();

        void ApplyChanges();
    }
}