namespace Orion.Zeta.Methods.Dev {
    public interface IDataService {
        T Retrieve<T>(string name);

        void Persist(string name, object data);
    }
}