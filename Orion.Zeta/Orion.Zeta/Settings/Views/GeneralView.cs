using System;
using System.Windows;
using System.Windows.Controls;
using Orion.Zeta.Methods.Dev.Setting;
using Orion.Zeta.Services;
using Orion.Zeta.Settings.Models;
using Orion.Zeta.Settings.ViewModels;
using Orion.Zeta.ViewModels;

namespace Orion.Zeta.Settings.Views {
    /// <summary>
    /// Interaction logic for GeneralView.xaml
    /// </summary>
    public partial class GeneralView : UserControl {
        public GeneralView(GeneralModel model) {
	        this.InitializeComponent();
            if (model.IsAutoRefreshEnbabled) {
                this.autoRefreshValue.Visibility = Visibility.Visible;
            }
        }

	    public GeneralView(IApplicationSettingService searchMethodSettingService, IModifiableGeneralSetting modifiableGeneralSetting) {
		    this.InitializeComponent();
			this.DataContext = new GeneralViewModel(searchMethodSettingService, modifiableGeneralSetting);
		}

		public void autoRefresh_OnIsCheckedChanged(object sender, EventArgs eventArgs) {
            if (this.autoRefresh.IsChecked.HasValue && this.autoRefresh.IsChecked.Value) {
                this.autoRefreshValue.Visibility = Visibility.Visible;
            } else {
                this.autoRefreshValue.Visibility = Visibility.Collapsed;
            }
        }
    }
}
