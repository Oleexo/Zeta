using System;
using System.Collections.Generic;
using Orion.Zeta.Core.SearchMethods;
using Orion.Zeta.Core.SearchMethods.ApplicationSearch;
using Orion.Zeta.Settings.Models;

namespace Orion.Zeta.Core.Settings.SearchMethods {
    public class ApplicationSearchSettingConnector : IModifiableSettings {
        private readonly ApplicationSearchMethod _applicationSearchMethod;

        public ApplicationSearchSettingConnector(ApplicationSearchMethod applicationSearchMethod) {
            this._applicationSearchMethod = applicationSearchMethod;
        }

        public void ApplyChanges(object item) {
            var model = item as ApplicationSearchModel;
            if (model == null)
                throw new ArgumentNullException(nameof(item));
            this.ApplyDirectories(model.Directories);
        }

        private void ApplyDirectories(IEnumerable<Directory> directories) {
            foreach (var directory in directories) {
                if (!this._applicationSearchMethod.IsRegistered(directory.Path)) {
                    this._applicationSearchMethod.RegisterPath(directory.Path, directory.Extensions);
                    this._applicationSearchMethod.Initialisation();
                }
            }
        }
    }
}