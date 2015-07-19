using System.Windows.Controls;
using Orion.Zeta.Methods.Dev;

namespace Orion.Zeta.Methods.Ui.Dev {
    public interface IMethodAsyncContainer : IBaseMethodContainer {
        // Search Method
        ISearchMethodAsync SearchMethod { get; }
    }
}