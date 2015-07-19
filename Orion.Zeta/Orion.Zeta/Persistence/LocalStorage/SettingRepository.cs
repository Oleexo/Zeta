using System;
using System.IO;
using Newtonsoft.Json;

namespace Orion.Zeta.Persistence.LocalStorage {
    public class SettingRepository : ISettingRepository {
        private string _basePath;
        private readonly string _path;

        public SettingRepository() {
            this._basePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            this._path = Path.Combine(Path.Combine(this._basePath, "Zeta"), "Settings");
            if (!Directory.Exists(this._path)) {
                Directory.CreateDirectory(this._path);
            }
        }

        public void Persite(string id, object data) {
            try {
                using (var writer = new StreamWriter(Path.Combine(this._path, id + ".json")))
                using (var jw = new JsonTextWriter(writer)) {
                    jw.Formatting = Formatting.Indented;

                    var serializer = new JsonSerializer();

                    serializer.Serialize(jw, data);
                }
            } catch (Exception) {
                throw new PersistenceException("persist fail");
            }
        }

        public T Find<T>(string id) {
            T data;
            try {
                using (var reader = new StreamReader(Path.Combine(this._path, id + ".json")))
                using (var jw = new JsonTextReader(reader)) {

                    var serializer = new JsonSerializer();

                    data = serializer.Deserialize<T>(jw);
                }
            } catch (Exception) {
                return default(T);
            }
            return data;
        }
    }
}