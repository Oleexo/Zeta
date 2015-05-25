using System.Collections.Generic;
using System.Threading.Tasks;
using Orion.Zeta.Core.Settings;
using Orion.Zeta.Persistence;

namespace Orion.Zeta.Services {
    public class SettingsService {
        private readonly ISettingRepository _settingRepository;
        private List<ISettingContainer> _settingContainers;

        public SettingsService(ISettingRepository settingRepository) {
            this._settingRepository = settingRepository;
            this._settingContainers = new List<ISettingContainer>();
        }

        public IEnumerable<ISettingContainer> GetSettingContainers() {
            return this._settingContainers;
        }

        public void RegisterGlobal(ISettingContainer settingContainer) {
            this._settingContainers.Add(settingContainer);
            if (!settingContainer.HaveDefaultData()) {
                settingContainer.ReadData(this._settingRepository);
            }
        }

        public void Register(ISettingContainer settingContainer) {
            this._settingContainers.Add(settingContainer);
        }

        public void ApplyChanges() {
            foreach (var settingContainer in this._settingContainers) {
                settingContainer.ApplyChanges();
            }
        }

        public void SaveChanges() {
            foreach (var settingContainer in this._settingContainers) {
                settingContainer.WriteData(this._settingRepository);
            }
        }

        public async Task SaveChangesAsync() {
            await Task.Run((() => this.SaveChanges()));
        }
    }
}