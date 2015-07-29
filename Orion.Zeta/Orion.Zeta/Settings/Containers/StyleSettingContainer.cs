using System.Windows.Controls;
using Orion.Zeta.Services;
using Orion.Zeta.Settings.Models;
using Orion.Zeta.Settings.Views;
using Orion.Zeta.ViewModels;

namespace Orion.Zeta.Settings.Containers {
	public class StyleSettingContainer : IApplicationSettingContainer {
		private readonly ApplicationSettingService _applicationSettingService;
		private readonly IModifiableStyleSetting _modifiableStyleSetting;

		public StyleSettingContainer(ApplicationSettingService applicationSettingService, IModifiableStyleSetting modifiableStyleSetting) {
			this._applicationSettingService = applicationSettingService;
			this._modifiableStyleSetting = modifiableStyleSetting;
		}

		public string Header => "Style";

		public bool? Enabled { get; set; }

		public UserControl CreateControl() {
			return new StyleView(this._applicationSettingService, this._modifiableStyleSetting);
		}

		public void OnCloseControl() {
			this._applicationSettingService.OnClosing();
		}

		public void ApplyConfiguration() {
			var model = this._applicationSettingService.Retrieve<StyleModel>("ApplicationStyle") ?? this.DefaultData();
			this._modifiableStyleSetting.UseNoneWindowStyle = model.IsSlimDesign;
			this._modifiableStyleSetting.Width = model.Width;
		}

		private StyleModel DefaultData() {
			return new StyleModel();
		}
	}
}