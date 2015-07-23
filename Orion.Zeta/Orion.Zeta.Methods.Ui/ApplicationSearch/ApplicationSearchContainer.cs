using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using Orion.Zeta.Methods.ApplicationSearch;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Dev.Setting;
using Orion.Zeta.Methods.Ui.ApplicationSearch.Models;
using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Methods.Ui.ApplicationSearch {
    [Export(typeof(IMethodAsyncContainer))]
    public class ApplicationSearchContainer : IMethodAsyncContainer {
        public bool HaveSettingControl => true;

        public UserControl CreateSettingControl(ISearchMethodSettingService searchMethodSettingService) {
            return new ApplicationSearchView(searchMethodSettingService);
        }

        public string Name => "Applications";
	    ISearchMethod IMethodContainer.GetNewInstanceOfSearchMethod(IDataService dataService) {
		    return this.GetNewInstanceOfSearchMethod(dataService);
	    }

	    public ISearchMethodAsync GetNewInstanceOfSearchMethod(IDataService dataService) {
			var searchMethod = new ApplicationSearchMethod();
		    var model = dataService.Retrieve<ApplicationSearchModel>("config");
		    this.ApplyConfiguration(searchMethod, model ?? this.DefaultData());
		    return searchMethod;
	    }

	    private ApplicationSearchModel DefaultData() {
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

	    private void ApplyConfiguration(ApplicationSearchMethod searchMethod, ApplicationSearchModel model) {
			foreach (var directory in model.Directories) {
				searchMethod.RegisterPath(directory.Path, directory.Extensions);
			}
		}
	}
}