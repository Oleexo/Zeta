using System.IO;
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

        public T Retrieve<T>(string name) {
	        var path = Path.Combine(this._baseFolderName, name);
	        return this._settingRepository.Find<T>(path);
        }

        public void Persist(string name, object data) {
			var path = Path.Combine(this._baseFolderName, name);
			this._settingRepository.Persite(path, data);
		}
    }
}