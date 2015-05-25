namespace Orion.Zeta.Persistence {
    public interface ISettingRepository {
        void Persite(string id, object data);
        object Find<T>(string id);
    }
}