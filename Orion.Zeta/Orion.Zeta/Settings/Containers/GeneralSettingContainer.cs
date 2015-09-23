using System.Windows.Controls;
using Orion.Zeta.Services;
using Orion.Zeta.Settings.Models;
using Orion.Zeta.Settings.Views;

namespace Orion.Zeta.Settings.Containers {
	public class GeneralSettingContainer : IApplicationSettingContainer {
		private readonly IApplicationSettingService _applicationSettingService;
		private readonly IModifiableGeneralSetting _modifiableGeneralSetting;

		public GeneralSettingContainer(IApplicationSettingService applicationSettingService, IModifiableGeneralSetting modifiableGeneralSetting) {
			this._applicationSettingService = applicationSettingService;
			this._modifiableGeneralSetting = modifiableGeneralSetting;
		}

		public string Header => "General";

		public bool? Enabled { get; set; }

		public UserControl CreateControl() {
			return new GeneralView(this._applicationSettingService, this._modifiableGeneralSetting);
		}

		public void OnCloseControl() {
			this._applicationSettingService.OnClosing();
		}

		public void ApplyConfiguration() {
			var model = this._applicationSettingService.Retrieve<GeneralModel>("ApplicationConfiguration") ?? this.DefaultData();
			this._modifiableGeneralSetting.StartOnBoot = model.IsStartOnBoot;
			if (model.IsAutoRefreshEnbabled) {
				this._modifiableGeneralSetting.EnabledAutoRefresh(model.AutoRefresh);
			}
		}

		private GeneralModel DefaultData() {
			return new GeneralModel();
		}
	}
}