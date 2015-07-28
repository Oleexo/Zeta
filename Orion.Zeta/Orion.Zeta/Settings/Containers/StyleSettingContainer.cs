using System.Windows.Controls;
using Orion.Zeta.Core.Settings;
using Orion.Zeta.Services;
using Orion.Zeta.ViewModels;

namespace Orion.Zeta.Settings.Containers {
	public class StyleSettingContainer : IApplicationSettingContainer {
		private readonly ApplicationSettingService _applicationSettingService;
		private readonly IModifiableStyleSetting _mainViewModel;

		public StyleSettingContainer(ApplicationSettingService applicationSettingService, IModifiableStyleSetting mainViewModel) {
			this._applicationSettingService = applicationSettingService;
			this._mainViewModel = mainViewModel;
		}

		public string Header => "Style";

		public bool? Enabled { get; set; }

		public UserControl CreateControl() {
			// TODO
			return null;
		}

		public void OnCloseControl() {
			// TODO
		}

		public void ApplyConfiguration() {
			// TODO
		}
	}
}