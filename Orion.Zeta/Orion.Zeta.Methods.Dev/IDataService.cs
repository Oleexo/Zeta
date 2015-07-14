namespace Orion.Zeta.Methods.Dev {
    public interface IDataService {
        object Retrieve(string name);

        void Persist(string name, object data);
    }
}