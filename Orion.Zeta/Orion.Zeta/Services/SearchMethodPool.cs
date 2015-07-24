using System;
using System.Collections.Generic;
using System.Linq;
using Orion.Zeta.Core;
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

		public bool ContainSearchMethod(string applicationName) {
			return this._instanceOfMethods.Any(i => i.Key.Name.Equals(applicationName));
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
}