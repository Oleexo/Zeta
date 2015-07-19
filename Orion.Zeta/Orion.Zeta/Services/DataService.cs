using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Persistence;

namespace Orion.Zeta.Services {
    public class DataService : IDataService {
        private readonly string _baseFolderName;
        private readonly ISettingRepository _settingRepository;

        public DataService(string baseFolderName, ISettingRepository settingRepository) {
            this._baseFolderName = baseFolderName;
            this._settingRepository = settingRepository;
        }

        public object Retrieve(string name) {
            throw new System.NotImplementedException();
        }

        public void Persist(string name, object data) {
            throw new System.NotImplementedException();
        }
    }
}