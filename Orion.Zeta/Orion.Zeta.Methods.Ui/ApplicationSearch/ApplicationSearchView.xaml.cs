using System.Windows.Controls;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Ui.ApplicationSearch.ViewModel;
using Orion.Zeta.Methods.Ui.Dev;

namespace Orion.Zeta.Methods.Ui.ApplicationSearch {
    /// <summary>
    /// Interaction logic for ApplicationSearchView.xaml
    /// </summary>
    public partial class ApplicationSearchView : UserControl {
        public ApplicationSearchView(IDataService dataService) {
            this.InitializeComponent();
            this.DataContext = new ApplicationSearchViewModel(dataService);
        }
    }
}
