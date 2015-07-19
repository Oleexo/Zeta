namespace Orion.Zeta.Persistence {
    public interface ISettingRepository {
        void Persite(string id, object data);
        T Find<T>(string id);
    }
}