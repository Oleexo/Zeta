using System;
using System.Collections.Generic;
using Orion.Zeta.Core.SearchMethods.ApplicationSearch;
using Orion.Zeta.Settings.Models;
using Orion.Zeta.Settings.Views;

namespace Orion.Zeta.Core.Settings.SearchMethods {
    public class ApplicationSearchMethodContainer : SearchMethodAsyncContainer {
        public ApplicationSearchMethodContainer() : base(new ApplicationSearchMethod()) {
            var applicationSearchModel = this.DefaultApplicationSearchModel();
            var applicationSearchSettingConnector = new ApplicationSearchSettingConnector(this.SearchMethod as ApplicationSearchMethod);
            this.SettingContainer = new DataSettingContainer<ApplicationSearchModel>("Application Search", typeof(ApplicationSearchView), applicationSearchSettingConnector, applicationSearchModel);
        }

        private ApplicationSearchModel DefaultApplicationSearchModel() {
            var model = new ApplicationSearchModel {
                Directories = new List<Directory> {
                    new Directory {
                        Path = Environment.GetFolderPath(Environment.SpecialFolder.Programs),
                        SpecialFolder = "Programs",
                        Extensions = new List<string> {
                            "*.exe",
                            "*.lnk"
                        }
                    },
                    new Directory {
                        Path = Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms),
                        SpecialFolder = "CommonPrograms",
                        Extensions = new List<string> {
                            "*.exe",
                            "*.lnk"
                        }
                    }
                }
            };
            return model;
        }
    }
}