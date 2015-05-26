using System.Collections.ObjectModel;
using Orion.Zeta.Settings.Models;
using Orion.Zeta.ViewModels;

namespace Orion.Zeta.Settings.ViewModels {
    public class ApplicationSearchViewModel : BaseViewModel {
        private readonly ApplicationSearchModel _model;

        public ObservableCollection<string> Directories { get; set; } 

        public ApplicationSearchViewModel(ApplicationSearchModel model) {
            this._model = model;
            this.Directories = new ObservableCollection<string>();
        }
    }
}