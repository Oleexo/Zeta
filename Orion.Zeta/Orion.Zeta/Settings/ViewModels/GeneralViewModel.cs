using Orion.Zeta.Settings.Models;
using Orion.Zeta.ViewModels;

namespace Orion.Zeta.Settings.ViewModels {
    public class GeneralViewModel : BaseViewModel {
        private readonly GeneralModel _model;

        public bool IsHideWhenLostFocus {
            get { return this._model.IsHideWhenLostFocus; }
            set {
                this._model.IsHideWhenLostFocus = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsAlwaysOnTop {
            get { return this._model.IsAlwaysOnTop; }
            set {
                this._model.IsAlwaysOnTop = value;
                this.OnPropertyChanged();
            }
        }

        public int AutoRefresh {
            get { return this._model.AutoRefresh; }
            set {
                this._model.AutoRefresh = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsAutoRefreshEnbabled {
            get { return this._model.IsAutoRefreshEnbabled; }
            set {
                this._model.IsAutoRefreshEnbabled = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsStartOnBoot {
            get { return this._model.IsStartOnBoot; }
            set {
                this._model.IsStartOnBoot = value;
                this.OnPropertyChanged();
            }
        }

        public GeneralViewModel(GeneralModel model) {
            this._model = model;
        }
    }
}