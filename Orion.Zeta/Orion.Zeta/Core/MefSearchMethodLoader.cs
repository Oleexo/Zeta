using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Core {
	public class MefSearchMethodLoader : ISearchMethodLoader {
		[ImportMany(typeof(IMethodContainer))]
		private IEnumerable<IMethodContainer> _searchMethods;
		[ImportMany(typeof(IMethodAsyncContainer))]
		private IEnumerable<IMethodAsyncContainer> _searchMethodsAsync;

		private string defaultPluginsFolder = @"Plugins/";

		public void Load(ICollection<IMethodContainer> searchMethods, ICollection<IMethodAsyncContainer> searchMethodsAsync) {
			var path = this.GetBasePath();
			try {
				var catalog = new AggregateCatalog();
				catalog.Catalogs.Add(new AssemblyCatalog(Path.Combine(path, "Orion.Zeta.Methods.Ui.dll")));
				if (Directory.Exists(this.defaultPluginsFolder)) {
					var directoryCatalog = new DirectoryCatalog(Path.Combine(path, this.defaultPluginsFolder));
					catalog.Catalogs.Add(directoryCatalog);
				}
				var container = new CompositionContainer(catalog);
				container.ComposeParts(this);
			}
			catch (Exception e) {
				Logger.LogError("Library loading fail", e);
				throw;
			}

			foreach (var methodContainer in this._searchMethods) {
				searchMethods.Add(methodContainer);
			}
			foreach (var methodAsyncContainer in this._searchMethodsAsync) {
				searchMethodsAsync.Add(methodAsyncContainer);
			}
		}

		private string GetBasePath() {
			return Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
		}
	}
}