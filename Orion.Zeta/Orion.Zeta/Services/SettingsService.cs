using System.Collections.Generic;
using System.Threading.Tasks;
using Orion.Zeta.Core.Settings;
using Orion.Zeta.Persistence;

namespace Orion.Zeta.Services {
    public class SettingsService {
        private readonly ISettingRepository _settingRepository;
        private readonly IList<ISettingContainer> _settingContainers;
        private readonly IList<IGeneralSettingContainer> _globalSettingContainers;
        private DataApplication _dataApplication;

	    public ISettingRepository SettingRepository => this._settingRepository;

	    public SettingsService(ISettingRepository settingRepository) {
            this._settingRepository = settingRepository;
            this._settingContainers = new List<ISettingContainer>();
            this._globalSettingContainers = new List<IGeneralSettingContainer>();
        }

        public IEnumerable<ISettingContainer> GetSettingContainers() {
            return this._settingContainers;
        }

        public IEnumerable<ISettingContainer> GetGlobalSettingContainers() {
            return this._globalSettingContainers;
        } 

        public void RegisterGlobal(IGeneralSettingContainer settingContainer) {
            this._globalSettingContainers.Add(settingContainer);
            settingContainer.ReadData(this._settingRepository);
            settingContainer.Enabled = null;
            settingContainer.ApplyChanges();
        }

        public void Register(ISettingContainer settingContainer) {
            settingContainer.Enabled = this.IsEnabled(settingContainer.Header);
            this._settingContainers.Add(settingContainer);
        }

        public bool IsEnabled(string header) {
            if (this._dataApplication == null) {
                this.LoadDataApplication();
            }
            if (this._dataApplication.EnableMethods.ContainsKey(header)) {
                return this._dataApplication.EnableMethods[header];
            }
            this._dataApplication.EnableMethods[header] = true;
            return true;
        }

        private void LoadDataApplication() {
            this._dataApplication = this._settingRepository.Find<DataApplication>("Zeta") ?? new DataApplication();
        }

        public void ApplyChanges() {
            foreach (var globalSettingContainer in this._globalSettingContainers) {
                globalSettingContainer.ApplyChanges();
            }
        }

        public void SaveChanges() {
            foreach (var globalSettingContainer in this._globalSettingContainers) {
                globalSettingContainer.WriteData(this._settingRepository);
            }
            this._settingRepository.Persite("Zeta", this._dataApplication);
        }

        public async Task SaveChangesAsync() {
            await Task.Run((() => this.SaveChanges()));
        }

        public async Task ApplyChangesAsync() {
            await Task.Run(() => this.ApplyChanges());
        }

        public void ToggleMethod(string header, bool? value) {
            if (value != null)
                this._dataApplication.EnableMethods[header] = value.Value;
        }
    }

    internal class DataApplication {
        public DataApplication() {
            this.EnableMethods = new Dictionary<string, bool>();
        }
        public Dictionary<string, bool> EnableMethods { get; set; }
    }
}