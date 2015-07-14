using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Orion.Zeta.Methods.Ui.Dev;
using Application = System.Windows.Application;

namespace Orion.Zeta.Methods.Ui.ApplicationSearch.Models {
    public class DirectoryModel : BaseViewModel {
        private readonly Directory _directory;

        public DirectoryModel() {
            this.Extensions = new ObservableCollection<string>();
            this._directory = new Directory();
        }

        public DirectoryModel(Directory directory) {
            this._directory = directory;
            this.Extensions = new ObservableCollection<string>(directory.Extensions);
            this.Extensions.CollectionChanged += (sender, args) => {
                if (args.Action == NotifyCollectionChangedAction.Add) {
                    foreach (var newItem in args.NewItems) {
                        this._directory.Extensions.Add(newItem as string);
                    }
                } else if (args.Action == NotifyCollectionChangedAction.Remove) {
                    foreach (var oldItem in args.OldItems) {
                        this._directory.Extensions.Remove(oldItem as string);
                    }
                }
            };
            this.ChangePathCommand = new RelayCommand(this.OnChangePathCommand);
            this.AddExtensionCommand = new RelayCommand(this.OnAddExtensionCommand);
            this.DeleteExtensionCommand = new RelayCommand(this.OnDeleteExtensionCommand);
        }

        public ObservableCollection<string> Extensions { get; set; }

        public string Path {
            get { return this._directory.Path; }
            set {
                this._directory.Path = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand ChangePathCommand { get; set; }

        public ICommand AddExtensionCommand { get; set; }

        public ICommand DeleteExtensionCommand { get; set; }

        private void OnDeleteExtensionCommand(object obj) {
            var item = obj as string;

            this.Extensions.Remove(item);
        }

        private async void OnAddExtensionCommand() {
            var metroWindow = Application.Current.Windows.OfType<MetroWindow>().SingleOrDefault(x => x.IsActive);
            
            var result = await metroWindow.ShowInputAsync("Adding extensions", "What extensions you want to add ?");
            if (result != null) {
                this.Extensions.Add(result);
            }
        }

        private void OnChangePathCommand() {
            var dialog = new FolderBrowserDialog();

            if (DialogResult.OK == dialog.ShowDialog()) {
                this.Path = dialog.SelectedPath;
            }
        }
    }
}