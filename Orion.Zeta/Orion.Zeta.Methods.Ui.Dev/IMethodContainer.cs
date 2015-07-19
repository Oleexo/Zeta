using Orion.Zeta.Methods.Dev;

namespace Orion.Zeta.Methods.Ui.Dev {
    public interface IMethodContainer : IBaseMethodContainer {
        // Search Method
        ISearchMethod SearchMethod { get; }
    }
}