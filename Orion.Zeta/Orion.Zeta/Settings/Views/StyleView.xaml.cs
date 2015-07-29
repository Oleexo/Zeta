using System.Windows.Controls;
using Orion.Zeta.Services;
using Orion.Zeta.Settings.ViewModels;
using Orion.Zeta.ViewModels;

namespace Orion.Zeta.Settings.Views {
    /// <summary>
    /// Interaction logic for StyleView.xaml
    /// </summary>
    public partial class StyleView : UserControl {
	    public StyleView(ApplicationSettingService applicationSettingService, IModifiableStyleSetting modifiableStyleSetting) {
		    this.InitializeComponent();
			this.DataContext = new StyleViewModel(applicationSettingService, modifiableStyleSetting);
		}
    }
}
