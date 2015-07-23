using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using Orion.Zeta.Core;
using Orion.Zeta.Core.Settings;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Services {
	public class SearchMethodPool : ISearchMethodPoolModifiable, ISearchMethodPoolLookable {
		private readonly ISearchEngine _searchEngine;
		private readonly Dictionary<IBaseMethodContainer, ISearchMethod> _instanceOfMethods;

		public SearchMethodPool(ISearchEngine searchEngine) {
			this._searchEngine = searchEngine;
			this._instanceOfMethods = new Dictionary<IBaseMethodContainer, ISearchMethod>();
		}

		public void Add(IMethodContainer methodContainer, SettingsService settingsService) {
			if (this._instanceOfMethods.ContainsKey(methodContainer)) {
				throw new Exception("Method already activated");
			}
			var instance = methodContainer.GetNewInstanceOfSearchMethod(new DataService(methodContainer.Name, settingsService.SettingRepository));
			this._instanceOfMethods.Add(methodContainer, instance);
			this._searchEngine.RegisterMethod(instance);
		}

		public void Remove(IBaseMethodContainer methodContainer) {
			if (!this._instanceOfMethods.ContainsKey(methodContainer)) {
				throw new Exception("Method not found");
			}
			this._searchEngine.UnRegister(this._instanceOfMethods[methodContainer]);
			this._instanceOfMethods.Remove(methodContainer);
		}

		public bool ContainSearchMethod(IBaseMethodContainer methodContainer) {
			return this._instanceOfMethods.ContainsKey(methodContainer);
		}

		public ISearchMethod GetInstanceOf(IBaseMethodContainer methodContainer) {
			ISearchMethod value;
			return !this._instanceOfMethods.TryGetValue(methodContainer, out value) ? null : value;
		}

		public ISearchMethod GetInstanceOf(string applicationName) {
			var value = this._instanceOfMethods.FirstOrDefault(i => i.Key.Name.Equals(applicationName));
			return value.Value;
		}
	}

	public interface ISearchMethodPoolLookable {
		bool ContainSearchMethod(IBaseMethodContainer methodContainer);
		ISearchMethod GetInstanceOf(IBaseMethodContainer methodContainer);
		ISearchMethod GetInstanceOf(string applicationName);
	}

	public interface ISearchMethodPoolModifiable {
		void Add(IMethodContainer methodContainer, SettingsService settingsService);

		void Remove(IBaseMethodContainer methodContainer);
	}

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