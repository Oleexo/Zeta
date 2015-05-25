using System.Collections.Generic;
using Orion.Zeta.Core;
using Orion.Zeta.Core.SearchMethods;

namespace Orion.Zeta.Services {
    public class SearchMethodService {
        private readonly SettingsService _settingsService;
        private List<SearchMethodContainer> _searchMethods;
        private List<SearchMethodAsyncContainer> _searchMethodsAsync;

        public SearchMethodService(SettingsService settingsService) {
            this._settingsService = settingsService;
            this._searchMethods = new List<SearchMethodContainer>();
            this._searchMethodsAsync = new List<SearchMethodAsyncContainer>();
        }

        public void RegisterSearchMethods(ISearchEngine searchEngine) {
            foreach (var searchMethod in this._searchMethods) {
                searchEngine.RegisterMethod(searchMethod.SearchMethod);
            }

            foreach (var searchMethodAsync in this._searchMethodsAsync) {
                searchEngine.RegisterMethod(searchMethodAsync.SearchMethod);
            }
        }

        public void AddContainer(SearchMethodAsyncContainer searchMethodAsyncContainer) {
            this._searchMethodsAsync.Add(searchMethodAsyncContainer);
            if (searchMethodAsyncContainer.IsModifiable())
                this._settingsService.Register(searchMethodAsyncContainer.SettingContainer);
        }

        public void AddContainer(SearchMethodContainer searchMethodContainer) {
            this._searchMethods.Add(searchMethodContainer);
            if (searchMethodContainer.IsModifiable())
                this._settingsService.Register(searchMethodContainer.SettingContainer);
        }
    }
}