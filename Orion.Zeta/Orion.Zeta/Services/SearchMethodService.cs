using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Orion.Zeta.Core;
using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Services {
    public class SearchMethodService {
        private readonly SettingsService _settingsService;
        [ImportMany(typeof(IMethodContainer))]
        private IEnumerable<IMethodContainer> _searchMethods;
        [ImportMany(typeof(IMethodAsyncContainer))]
        private IEnumerable<IMethodAsyncContainer> _searchMethodsAsync;

        public SearchMethodService(SettingsService settingsService) {
            this._settingsService = settingsService;
            this.Initialisation();
        }

        private void Initialisation() {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog("Orion.Zeta.Methods.Ui.dll"));
            var directoryCatalog = new DirectoryCatalog(@"Plugins/");
            catalog.Catalogs.Add(directoryCatalog);
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        public void RegisterSearchMethods(ISearchEngine searchEngine) {
            foreach (var searchMethod in this._searchMethods) {
                searchEngine.RegisterMethod(searchMethod.SearchMethod);
            }

            foreach (var searchMethodAsync in this._searchMethodsAsync) {
                searchEngine.RegisterMethod(searchMethodAsync.SearchMethod);
            }
        }

        public void ToggleMethodBySetting() {
            foreach (var searchMethod in this._searchMethods) {
            }
        }
    }
}