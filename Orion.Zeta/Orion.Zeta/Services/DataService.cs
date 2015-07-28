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
			var path = this.GetPath(name);
			return this._settingRepository.Find<T>(path);
		}

		public void Persist(string name, object data) {
			var path = this.GetPath(name);
			this._settingRepository.Persite(path, data);
		}

		private string GetPath(string name) {
			if (this._applicationName == null) {
				return name;
			}
			return Path.Combine(this._applicationName, name);
		}
	}
}