using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Ui.ApplicationSearch.Models;
using Orion.Zeta.Methods.Ui.Dev;
using Orion.Zeta.Settings.Models;
using Application = System.Windows.Application;

namespace Orion.Zeta.Methods.Ui.ApplicationSearch.ViewModel {
    public class ApplicationSearchViewModel : BaseViewModel {
        private readonly ApplicationSearchModel _model;
        private IDataService dataService;

        public ObservableCollection<DirectoryModel> Directories { get; set; } 

        public ICommand AddDirectoryCommand { get; private set; }

        public ICommand RemoveDirectoryCommand { get; private set; }

        public ApplicationSearchViewModel(ApplicationSearchModel model) {
            this._model = model;
            this.Directories = new ObservableCollection<DirectoryModel>();
            foreach (var directory in model.Directories) {
                this.Directories.Add(new DirectoryModel(directory));
            }
            this.AddDirectoryCommand = new RelayCommand(this.OnAddDirectoryCommand);
            this.RemoveDirectoryCommand = new RelayCommand(this.OnRemoveDirectoryCommand);
        }

        public ApplicationSearchViewModel(IDataService dataService) {
            this.dataService = dataService;
        }

        private void OnRemoveDirectoryCommand(object data) {
            var item = data as DirectoryModel;
            if (item != null) {
                this.Directories.Remove(item);
                this._model.Directories.RemoveAll(d => d.Path.Equals(item.Path));
            }

        }

        private void OnAddDirectoryCommand() {
            var dialog = new FolderBrowserDialog();

            if (DialogResult.OK == dialog.ShowDialog()) {
                var path = dialog.SelectedPath;
                if (this.Directories.Any(d => d.Path.Equals(path))) {
                    var metroWindow = Application.Current.Windows.OfType<MetroWindow>().SingleOrDefault(x => x.IsActive);
                    metroWindow?.ShowMessageAsync("Directory", "this directory is already in list");
                    return;
                }
                var directory = new Directory {
                    Path = path
                };
                this._model.Directories.Add(directory);
                var directoryModel = new DirectoryModel(directory);
                this.Directories.Add(directoryModel);
            }
        }
    }
}