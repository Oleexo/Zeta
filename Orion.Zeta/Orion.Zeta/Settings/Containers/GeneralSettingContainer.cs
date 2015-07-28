using System.Windows.Controls;
using Orion.Zeta.Methods.Dev.Setting;
using Orion.Zeta.Services;
using Orion.Zeta.Settings.Models;
using Orion.Zeta.Settings.Views;
using Orion.Zeta.ViewModels;

namespace Orion.Zeta.Settings.Containers {
	public class GeneralSettingContainer : IApplicationSettingContainer {
		private readonly IApplicationSettingService _searchMethodSettingService;
		private readonly IModifiableGeneralSetting _modifiableGeneralSetting;

		public GeneralSettingContainer(IApplicationSettingService searchMethodSettingService, IModifiableGeneralSetting modifiableGeneralSetting) {
			this._searchMethodSettingService = searchMethodSettingService;
			this._modifiableGeneralSetting = modifiableGeneralSetting;
		}

		public string Header => "General";

		public bool? Enabled { get; set; }

		public UserControl CreateControl() {
			return new GeneralView(this._searchMethodSettingService, this._modifiableGeneralSetting);
		}

		public void OnCloseControl() {
			this._searchMethodSettingService.OnClosing();
		}

		public void ApplyConfiguration() {
			var model = this._searchMethodSettingService.Retrieve<GeneralModel>("ApplicationConfiguration");
			if (model == null) {
				model = this.DefaultData();
			}
			this._modifiableGeneralSetting.IsAlwaysOnTop = model.IsAlwaysOnTop;
			this._modifiableGeneralSetting.IsHideWhenLostFocus = model.IsHideWhenLostFocus;
			this._modifiableGeneralSetting.StartOnBoot = model.IsStartOnBoot;
			if (model.IsAutoRefreshEnbabled) {
				this._modifiableGeneralSetting.EnabledAutoRefresh(model.AutoRefresh);
			}
		}

		private GeneralModel DefaultData() {
			return new GeneralModel {
				AutoRefresh = 60,
				IsAutoRefreshEnbabled = true,
				IsAlwaysOnTop = true,
				IsHideWhenLostFocus = true,
				IsStartOnBoot = true
			};
		}
	}
}