using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Dev.Setting;

namespace Orion.Zeta.Methods.Ui.Dev {
    public interface IMethodContainer : IBaseMethodContainer {
	    ISearchMethod GetNewInstanceOfSearchMethod(IDataService dataService);
    }
}