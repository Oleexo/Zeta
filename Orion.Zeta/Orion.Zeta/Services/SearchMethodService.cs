using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using Orion.Zeta.Core.Settings;
using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Services {
	public class SearchMethodService : ISearchMethodService {
        [ImportMany(typeof(IMethodContainer))]
        private IEnumerable<IMethodContainer> _searchMethods;
        [ImportMany(typeof(IMethodAsyncContainer))]
        private IEnumerable<IMethodAsyncContainer> _searchMethodsAsync;

		private readonly SearchMethodPool _searchMethodPool;

        public SearchMethodService(SearchMethodPool searchMethodPool) {
	        this._searchMethodPool = searchMethodPool;
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

		/// <summary>
		/// Enable method in engine at initialisation
		/// </summary>
		/// <param name="searchEngine"></param>
		/// <param name="settingsService"></param>
        public void RegisterSearchMethods(SettingsService settingsService) {
            foreach (var searchMethod in this._searchMethods) {
				this.RegisterSearchMethod(searchMethod, settingsService);
            }

            foreach (var searchMethodAsync in this._searchMethodsAsync) {
				this.RegisterSearchMethod(searchMethodAsync, settingsService);
            }
        }

		private void RegisterSearchMethod(IMethodContainer searchMethod, SettingsService settingsService) {
			if (settingsService.IsEnabled(searchMethod.Name)) {
				this._searchMethodPool.Add(searchMethod, settingsService);
			}
		}

		/// <summary>
		/// Enable or disable method
		/// </summary>
		/// <param name="searchEngine"></param>
		/// <param name="settingsService"></param>
        public void ManageMethodsBySetting(SettingsService settingsService) {
            foreach (var searchMethod in this._searchMethods) {
                var status = settingsService.IsEnabled(searchMethod.Name);
                if (status) {
                    if (!this._searchMethodPool.ContainSearchMethod(searchMethod)) {
						this._searchMethodPool.Add(searchMethod, settingsService);
                    }
                }
                else {
                    if (this._searchMethodPool.ContainSearchMethod(searchMethod)) {
						this._searchMethodPool.Remove(searchMethod);
                    }
                }
            }
            foreach (var searchMethodAsync in this._searchMethodsAsync) {
                var status = settingsService.IsEnabled(searchMethodAsync.Name);
                if (status) {
                    if (!this._searchMethodPool.ContainSearchMethod(searchMethodAsync)) {
						this._searchMethodPool.Add(searchMethodAsync, settingsService);
                    }
                } else {
                    if (this._searchMethodPool.ContainSearchMethod(searchMethodAsync)) {
						this._searchMethodPool.Remove(searchMethodAsync);
                    }
                }
            }

        }

		public void ToggleMethod(IMethodContainer searchMethod, SettingsService settingsService) {
			if (this._searchMethodPool.ContainSearchMethod(searchMethod)) {
				this._searchMethodPool.Remove(searchMethod);
			} else {
				this._searchMethodPool.Add(searchMethod, settingsService);
			}
		}

		public void ToggleMethod(string searchMethodName, SettingsService settingsService, bool value) {
			if (this._searchMethodPool.ContainSearchMethod(searchMethodName) && !value) {
				this._searchMethodPool.Remove(this.FindByName(searchMethodName));
			}
			else if (!this._searchMethodPool.ContainSearchMethod(searchMethodName) && value) {
				this._searchMethodPool.Add(this.FindByName(searchMethodName), settingsService);
			}
		}

		private IMethodContainer FindByName(string searchMethodName) {
			var container = this._searchMethods.FirstOrDefault(sm => sm.Name.Equals(searchMethodName));
			if (container == null)
				return this._searchMethodsAsync.FirstOrDefault(sma => sma.Name.Equals(searchMethodName));
			return container;
		}

		/// <summary>
		/// Register method in setting panel
		/// </summary>
		/// <param name="settingsService"></param>
		public void RegisterSettings(SettingsService settingsService) {
            foreach (var methodContainer in this._searchMethods) {
                settingsService.Register(new SettingContainer(methodContainer, new SearchMethodSettingService(methodContainer.Name, settingsService, this._searchMethodPool)));
            }

            foreach (var methodAsyncContainer in this._searchMethodsAsync) {
                settingsService.Register(new SettingContainer(methodAsyncContainer, new SearchMethodSettingService(methodAsyncContainer.Name, settingsService, this._searchMethodPool)));
            }
        }
    }
}