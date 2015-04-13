namespace Orion.Zeta.Core {
	public interface IExecute {
		string Program { get; }

		string Parameters { get; }
		void Start();
	}
}