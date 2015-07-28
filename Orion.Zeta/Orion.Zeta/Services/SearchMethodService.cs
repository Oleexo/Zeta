using System.Collections.Generic;
using System.Linq;
using Orion.Zeta.Core;
using Orion.Zeta.Core.Settings;
using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Services {
	public class SearchMethodService : ISearchMethodService {
        private ICollection<IMethodContainer> _searchMethods;
        private ICollection<IMethodAsyncContainer> _searchMethodsAsync;

		private readonly SearchMethodPool _searchMethodPool;
		private readonly ISearchMethodLoader _methodsLoader;

		public SearchMethodService(SearchMethodPool searchMethodPool, ISearchMethodLoader methodsLoader) {
	        this._searchMethodPool = searchMethodPool;
			this._methodsLoader = methodsLoader;
			this.Initialisation();
        }

		private void Initialisation() {
			this._searchMethods = new List<IMethodContainer>();
			this._searchMethodsAsync = new List<IMethodAsyncContainer>();
			this._methodsLoader.Load(this._searchMethods, this._searchMethodsAsync);
        }

		/// <summary>
		/// Enable method in engine at initialisation
		/// </summary>
		/// <param name="settingsService"></param>
		public void RegisterSearchMethods(ISettingsService settingsService) {
            foreach (var searchMethod in this._searchMethods) {
				this.RegisterSearchMethod(searchMethod, settingsService);
            }

            foreach (var searchMethodAsync in this._searchMethodsAsync) {
				this.RegisterSearchMethod(searchMethodAsync, settingsService);
            }
        }

		private void RegisterSearchMethod(IMethodContainer searchMethod, ISettingsService settingsService) {
			if (!this.IsCorrectSearchMethodName(searchMethod.Name)) {
				this.RemoveSearchMethod(searchMethod);
			}
			if (settingsService.IsEnabled(searchMethod.Name)) {
				this._searchMethodPool.Add(searchMethod, settingsService);
			}
		}

		private void RemoveSearchMethod(IMethodContainer searchMethod) {
			if (this._searchMethods.Contains(searchMethod)) {
				this._searchMethods.Remove(searchMethod);
			}
		}

		private bool IsCorrectSearchMethodName(string searchMethodName) {
			// Todo protected against . & / in name
			if (string.IsNullOrWhiteSpace(searchMethodName)) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// Enable or disable method
		/// </summary>
		/// <param name="settingsService"></param>
		public void ManageMethodsBySetting(ISettingsService settingsService) {
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

		public void ToggleMethod(IMethodContainer searchMethod, ISettingsService settingsService) {
			if (this._searchMethodPool.ContainSearchMethod(searchMethod)) {
				this._searchMethodPool.Remove(searchMethod);
			} else {
				this._searchMethodPool.Add(searchMethod, settingsService);
			}
		}

		public void ToggleMethod(string searchMethodName, ISettingsService settingsService, bool value) {
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
		public void RegisterSettings(ISettingsService settingsService) {
            foreach (var methodContainer in this._searchMethods) {
                settingsService.Register(new SettingContainer(methodContainer, new SearchMethodSettingService(methodContainer.Name, settingsService, this._searchMethodPool)));
            }

            foreach (var methodAsyncContainer in this._searchMethodsAsync) {
                settingsService.Register(new SettingContainer(methodAsyncContainer, new SearchMethodSettingService(methodAsyncContainer.Name, settingsService, this._searchMethodPool)));
            }
        }
    }
}