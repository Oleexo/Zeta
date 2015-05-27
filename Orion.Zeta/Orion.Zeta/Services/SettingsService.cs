using System.Collections.Generic;
using System.Threading.Tasks;
using Orion.Zeta.Core.Settings;
using Orion.Zeta.Persistence;

namespace Orion.Zeta.Services {
    public class SettingsService {
        private readonly ISettingRepository _settingRepository;
        private readonly IList<ISettingContainer> _settingContainers;
        private readonly IList<ISettingContainer> _globalSettingContainers;

        public SettingsService(ISettingRepository settingRepository) {
            this._settingRepository = settingRepository;
            this._settingContainers = new List<ISettingContainer>();
            this._globalSettingContainers = new List<ISettingContainer>();
        }

        public IEnumerable<ISettingContainer> GetSettingContainers() {
            return this._settingContainers;
        }

        public IEnumerable<ISettingContainer> GetGlobalSettingContainers() {
            return this._globalSettingContainers;
        } 

        public void RegisterGlobal(ISettingContainer settingContainer) {
            this._globalSettingContainers.Add(settingContainer);
            settingContainer.ReadData(this._settingRepository);
            settingContainer.Enabled = null;
            settingContainer.ApplyChanges();
        }

        public void Register(ISettingContainer settingContainer) {
            this._settingContainers.Add(settingContainer);
            settingContainer.ReadData(this._settingRepository);
        }

        public void ApplyChanges() {
            foreach (var globalSettingContainer in this._globalSettingContainers) {
                globalSettingContainer.ApplyChanges();
            }

            foreach (var settingContainer in this._settingContainers) {
                settingContainer.ApplyChanges();
            }
        }

        public void SaveChanges() {
            foreach (var globalSettingContainer in this._globalSettingContainers) {
                globalSettingContainer.WriteData(this._settingRepository);
            }

            foreach (var settingContainer in this._settingContainers) {
                settingContainer.WriteData(this._settingRepository);
            }
        }

        public async Task SaveChangesAsync() {
            await Task.Run((() => this.SaveChanges()));
        }

        public async Task ApplyChangesAsync() {
            await Task.Run(() => this.ApplyChanges());
        }
    }
}