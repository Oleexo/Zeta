using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using Orion.Zeta.Methods.Ui.Dev.MethodContainers;

namespace Orion.Zeta.Core {
	public class MefSearchMethodLoader : ISearchMethodLoader {
		[ImportMany(typeof(IMethodContainer))]
		private IEnumerable<IMethodContainer> _searchMethods;
		[ImportMany(typeof(IMethodAsyncContainer))]
		private IEnumerable<IMethodAsyncContainer> _searchMethodsAsync;

		private string defaultPluginsFolder = @"Plugins/";

		public void Load(ICollection<IMethodContainer> searchMethods, ICollection<IMethodAsyncContainer> searchMethodsAsync) {
			var catalog = new AggregateCatalog();
			catalog.Catalogs.Add(new AssemblyCatalog("Orion.Zeta.Methods.Ui.dll"));
			if (Directory.Exists(this.defaultPluginsFolder)) {
				var directoryCatalog = new DirectoryCatalog(this.defaultPluginsFolder);
				catalog.Catalogs.Add(directoryCatalog);
			}
			var container = new CompositionContainer(catalog);
			container.ComposeParts(this);

			foreach (var methodContainer in this._searchMethods) {
				searchMethods.Add(methodContainer);
			}
			foreach (var methodAsyncContainer in this._searchMethodsAsync) {
				searchMethodsAsync.Add(methodAsyncContainer);
			}
		}
	}
}