using System;
using System.Windows;
using System.Windows.Threading;
using Orion.Zeta.Methods.Ui.Dev.ViewModel;
using Orion.Zeta.Services;
using Orion.Zeta.Settings.Models;

namespace Orion.Zeta.Settings.ViewModels {
	public class GeneralViewModel : SettingBaseViewModel<GeneralModel> {
		private readonly IModifiableGeneralSetting _modifiableGeneralSetting;

		public int AutoRefresh {
			get { return this._model.AutoRefresh; }
			set {
				this._model.AutoRefresh = value;
				this.ModelModified();
				this.OnPropertyChanged();
				this._modifiableGeneralSetting.EnabledAutoRefresh(value);
			}
		}

		public bool IsAutoRefreshEnbabled {
			get { return this._model.IsAutoRefreshEnbabled; }
			set {
				this._model.IsAutoRefreshEnbabled = value;
				this.ModelModified();
				this.OnPropertyChanged();
				if (value) {
					this._modifiableGeneralSetting.EnabledAutoRefresh(this.AutoRefresh);
				}
				else {
					this._modifiableGeneralSetting.DisabledAutoRefresh();
				}
			}
		}

		public bool IsStartOnBoot {
			get { return this._model.IsStartOnBoot; }
			set {
				this._model.IsStartOnBoot = value;
				this.ModelModified();
				this.OnPropertyChanged();
				this._modifiableGeneralSetting.StartOnBoot = value;
			}
		}

		public GeneralViewModel(IApplicationSettingService applicationSettingService, IModifiableGeneralSetting modifiableGeneralSetting) : base(applicationSettingService, "ApplicationConfiguration") {
			this._modifiableGeneralSetting = modifiableGeneralSetting;
			applicationSettingService.Closing += (sender, args) => this.SaveDataSetting();
			this.Initialise();
		}

		private void Initialise() {
			this.LoadDataSettingAsync().ContinueWith((result) => {
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(this.ApplyDataLoaded));
			});
		}

		private void ApplyDataLoaded() {
			this.OnPropertyChanged("IsStartOnBoot");
			this.OnPropertyChanged("IsAutoRefreshEnbabled");
			this.OnPropertyChanged("AutoRefresh");
		}
	}
}