using System.Windows.Controls;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Dev.Setting;
using Orion.Zeta.Methods.Ui.ApplicationSearch.ViewModel;

namespace Orion.Zeta.Methods.Ui.ApplicationSearch {
    /// <summary>
    /// Interaction logic for ApplicationSearchView.xaml
    /// </summary>
    public partial class ApplicationSearchView : UserControl {
        public ApplicationSearchView(ISearchMethodSettingService searchMethodSettingService) {
            this.InitializeComponent();
            this.DataContext = new ApplicationSearchViewModel(searchMethodSettingService);
        }
    }
}
