using System.IO;
using Orion.Zeta.Methods.Dev.Setting;
using Orion.Zeta.Persistence;

namespace Orion.Zeta.Services {
	public class DataService : IDataService {
		private readonly string _applicationName;
		private readonly ISettingRepository _settingRepository;

		public DataService(string applicationName, ISettingRepository settingRepository) {
			this._applicationName = applicationName;
			this._settingRepository = settingRepository;
		}
		public T Retrieve<T>(string name) {
			var path = Path.Combine(this._applicationName, name);
			return this._settingRepository.Find<T>(path);
		}

		public void Persist(string name, object data) {
			var path = Path.Combine(this._applicationName, name);
			this._settingRepository.Persite(path, data);
		}
	}
}