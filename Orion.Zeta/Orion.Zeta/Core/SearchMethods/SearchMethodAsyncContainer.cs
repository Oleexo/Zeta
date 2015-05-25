using System;
using Orion.Zeta.Core.Settings;

namespace Orion.Zeta.Core.SearchMethods {
    public class SearchMethodAsyncContainer : SearchMethodContainerBase {
        public SearchMethodAsyncContainer(ISearchMethodAsync searchMethodAsync, ISettingContainer settingContainer) {
            this.SettingContainer = settingContainer;
            this.SearchMethod = searchMethodAsync;
        }

        public SearchMethodAsyncContainer(ISearchMethodAsync searchMethodAsync) {
            this.SettingContainer = null;
            this.SearchMethod = searchMethodAsync;
        }

        public ISearchMethodAsync SearchMethod { get; set; }

        public SearchMethodAsyncContainer AddSettings<T>(string name, Type type) where T : class, new() {
            if (!(this.SearchMethod is IModifiableSettings))
                throw new Exception("Impossible to assign search method to modifiable setting");
            this.SettingContainer = new DataSettingContainer<T>(name, type, (IModifiableSettings) this.SearchMethod);
            return this;
        }
    }
}