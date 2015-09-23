using System;
using System.Windows;
using System.Windows.Threading;
using Orion.Zeta.Methods.Ui.Dev;
using Orion.Zeta.Methods.Ui.Dev.ViewModel;
using Orion.Zeta.Services;
using Orion.Zeta.Settings.Models;
using Orion.Zeta.ViewModels;

namespace Orion.Zeta.Settings.ViewModels {
	public class StyleViewModel : SettingBaseViewModel<StyleModel> {
		private readonly IModifiableStyleSetting _modifiableStyleSetting;

		public bool IsSlimDesign {
			get { return this._model.IsSlimDesign; }
			set {
				this._model.IsSlimDesign = value;
				this.ModelModified();
				this.OnPropertyChanged();
			}
		}

		public double Width
		{
			get { return this._model.Width; }
			set
			{
				this._model.Width = value;
				this._modifiableStyleSetting.Width = value;
				this.ModelModified();
				this.OnPropertyChanged();
			}
		}

		public bool IsHideWhenLostFocus {
			get { return this._model.IsHideWhenLostFocus; }
			set {
				this._model.IsHideWhenLostFocus = value;
				this.ModelModified();
				this.OnPropertyChanged();
				this._modifiableStyleSetting.IsHideWhenLostFocus = value;
			}
		}

		public bool IsAlwaysOnTop {
			get { return this._model.IsAlwaysOnTop; }
			set {
				this._model.IsAlwaysOnTop = value;
				this.ModelModified();
				this.OnPropertyChanged();
				this._modifiableStyleSetting.IsAlwaysOnTop = value;
			}
		}

		public StyleViewModel(ApplicationSettingService applicationSettingService, IModifiableStyleSetting modifiableStyleSetting) : base(applicationSettingService, "ApplicationStyle") {
			this._modifiableStyleSetting = modifiableStyleSetting;
			applicationSettingService.Closing += (sender, args) => this.SaveDataSetting();
			this.Initialise();
		}

		private void Initialise() {
			this.LoadDataSettingAsync().ContinueWith((result) => {
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(this.ApplyDataLoaded));
			});
		}

		private void ApplyDataLoaded() {
			this.OnPropertyChanged("IsSlimDesign");
			this.OnPropertyChanged("Width");
			this.OnPropertyChanged("IsAlwaysOnTop");
			this.OnPropertyChanged("IsHideWhenLostFocus");
		}
	}
}