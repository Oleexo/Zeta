using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Orion.Zeta.Methods.ApplicationSearch;
using Orion.Zeta.Methods.Dev.Setting;
using Orion.Zeta.Methods.Ui.ApplicationSearch.Models;
using Orion.Zeta.Methods.Ui.Dev;
using Orion.Zeta.Methods.Ui.Dev.Tools.MVVM;
using Orion.Zeta.Methods.Ui.Dev.ViewModel;
using Application = System.Windows.Application;

namespace Orion.Zeta.Methods.Ui.ApplicationSearch.ViewModel {
	public class ApplicationSearchViewModel : SearchMethodSettingBaseViewModel<ApplicationSearchMethod, ApplicationSearchModel> {
		private DirectoryModel _currentDirectorySelected;
		private string _currentExtensionSelected;
		public ObservableCollection<DirectoryModel> Directories { get; set; }

		public DirectoryModel CurrentDirectorySelected {
			get { return this._currentDirectorySelected; }
			set {
				this._currentDirectorySelected = value;
				this.OnPropertyChanged("CurrentExtensionsOfDirectorySelected");
				this.OnPropertyChanged();
			}
		}

		public ObservableCollection<string> CurrentExtensionsOfDirectorySelected {
			get { return this.CurrentDirectorySelected?.Extensions; }
		}

		public string CurrentExtensionSelected {
			get { return this._currentExtensionSelected; }
			set {
				this._currentExtensionSelected = value;
				this.OnPropertyChanged();
			}
		}

		public ICommand AddDirectoryCommand { get; private set; }

		public ICommand RemoveDirectoryCommand { get; private set; }

		public ICommand AddExtensionCommand { get; private set; }

		public ICommand RemoveExtensionCommand { get; private set; }

		public ICommand ChangePathDirectoryCommand { get; private set; }

		public ApplicationSearchViewModel(ISearchMethodSettingService searchMethodSettingService) : base(searchMethodSettingService) {
			this.SearchMethodSettingService.Closing += (sender, args) => {
				this.SaveDataSettingAsync();
			};
			this.Directories = new ObservableCollection<DirectoryModel>();
			this.AddDirectoryCommand = new RelayCommand(this.OnAddDirectoryCommand);
			this.RemoveDirectoryCommand = new RelayCommand(this.OnRemoveDirectoryCommand);
			this.AddExtensionCommand = new RelayCommand(this.OnAddExtensionCommand);
			this.RemoveExtensionCommand = new RelayCommand(this.OnRemoveExtensionCommand);
			this.ChangePathDirectoryCommand = new RelayCommand(this.OnChangePathDirectoryCommand);
			this.Initialise();
		}

		private void Initialise() {
			this.LoadDataSettingAsync().ContinueWith((result) => {
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(this.ApplyDataLoaded));
			});
		}

		private void ApplyDataLoaded() {
			if (this._model.Directories != null) {
				foreach (var directory in this._model.Directories) {
					this.Directories.Add(new DirectoryModel(directory));
				}
			}
		}

		private void OnRemoveDirectoryCommand() {
			if (this.CurrentDirectorySelected != null) {
				this.Directories.Remove(this.CurrentDirectorySelected);
				this._model.Directories.RemoveAll(d => d.Path.Equals(this.CurrentDirectorySelected.Path));
				this.ModelModified();
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
				this.SearchMethod?.RegisterPath(directory.Path, directory.Extensions);
				this.ModelModified();
			}
		}

		private async void OnAddExtensionCommand() {
			var metroWindow = Application.Current.Windows.OfType<MetroWindow>().SingleOrDefault(x => x.IsActive);

			var result = await metroWindow.ShowInputAsync("Adding extensions", "What extensions you want to add ?");
			if (result != null) {
				this.CurrentDirectorySelected.Extensions.Add(result);
				this.ModelModified();
			}
		}

		private void OnRemoveExtensionCommand() {
			var extension = this.CurrentExtensionSelected;
			this.CurrentExtensionSelected = null;
			this.CurrentDirectorySelected.Extensions.Remove(extension);
			this.ModelModified();
		}

		private void OnChangePathDirectoryCommand() {
			if (this.CurrentDirectorySelected == null)
				return ;
			var dialog = new FolderBrowserDialog { SelectedPath = this.CurrentDirectorySelected.Path };

			if (DialogResult.OK == dialog.ShowDialog()) {
				this.CurrentDirectorySelected.Path = dialog.SelectedPath;
				this.ModelModified();
			}
		}
	}
}