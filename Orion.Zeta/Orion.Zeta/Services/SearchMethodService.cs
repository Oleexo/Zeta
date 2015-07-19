using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Orion.Zeta.Core;
using Orion.Zeta.Core.Settings;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Services {
    public interface ISearchMethodService {
        void RegisterSearchMethods(ISearchEngine searchEngine, SettingsService settingsService);

        void ToggleMethodBySetting(ISearchEngine searchEngine, SettingsService settingsService);

        void RegisterSettings(SettingsService settingsService);
    }

    public class SearchMethodService : ISearchMethodService {
        [ImportMany(typeof(IMethodContainer))]
        private IEnumerable<IMethodContainer> _searchMethods;
        [ImportMany(typeof(IMethodAsyncContainer))]
        private IEnumerable<IMethodAsyncContainer> _searchMethodsAsync;

        private Dictionary<IBaseMethodContainer, ISearchMethod> _currentInstanceOfMethods; 

        public SearchMethodService() {
            this._currentInstanceOfMethods = new Dictionary<IBaseMethodContainer, ISearchMethod>();
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

        public void RegisterSearchMethods(ISearchEngine searchEngine, SettingsService settingsService) {
            foreach (var searchMethod in this._searchMethods) {
                if (settingsService.IsEnabled(searchMethod.Name)) {
                    var instance = searchMethod.SearchMethod;
                    this._currentInstanceOfMethods.Add(searchMethod, instance);
                    searchEngine.RegisterMethod(instance);
                }
            }

            foreach (var searchMethodAsync in this._searchMethodsAsync) {
                if (settingsService.IsEnabled(searchMethodAsync.Name)) {
                    var instance = searchMethodAsync.SearchMethod;
                    this._currentInstanceOfMethods.Add(searchMethodAsync, instance);
                    searchEngine.RegisterMethod(instance);
                }
            }
        }

        public void ToggleMethodBySetting(ISearchEngine searchEngine, SettingsService settingsService) {
            foreach (var searchMethod in this._searchMethods) {
                var status = settingsService.IsEnabled(searchMethod.Name);
                if (status) {
                    if (!this._currentInstanceOfMethods.ContainsKey(searchMethod)) {
                        var instance = searchMethod.SearchMethod;
                        this._currentInstanceOfMethods.Add(searchMethod, instance);
                        searchEngine.RegisterMethod(instance);
                    }
                }
                else {
                    if (this._currentInstanceOfMethods.ContainsKey(searchMethod)) {
                        searchEngine.UnRegister(this._currentInstanceOfMethods[searchMethod]);
                        this._currentInstanceOfMethods.Remove(searchMethod);
                    }
                }
            }
            foreach (var searchMethodAsync in this._searchMethodsAsync) {
                var status = settingsService.IsEnabled(searchMethodAsync.Name);
                if (status) {
                    if (!this._currentInstanceOfMethods.ContainsKey(searchMethodAsync)) {
                        var instance = searchMethodAsync.SearchMethod;
                        this._currentInstanceOfMethods.Add(searchMethodAsync, instance);
                        searchEngine.RegisterMethod(instance);
                    }
                } else {
                    if (this._currentInstanceOfMethods.ContainsKey(searchMethodAsync)) {
                        searchEngine.UnRegister(this._currentInstanceOfMethods[searchMethodAsync]);
                        this._currentInstanceOfMethods.Remove(searchMethodAsync);
                    }
                }
            }

        }

        public void RegisterSettings(SettingsService settingsService) {
            foreach (var searchMethod in this._searchMethods) {
                settingsService.Register(new SettingContainer(searchMethod, settingsService.For(searchMethod.Name), searchMethod.SearchMethod));
            }

            foreach (var searchMethodAsync in this._searchMethodsAsync) {
                settingsService.Register(new SettingContainer(searchMethodAsync, settingsService.For(searchMethodAsync.Name), searchMethodAsync.SearchMethod));
            }
        }
    }
}