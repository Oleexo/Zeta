using System.Windows.Controls;
using Orion.Zeta.Settings.Models;
using Orion.Zeta.Settings.ViewModels;

namespace Orion.Zeta.Settings.Views {
    /// <summary>
    /// Interaction logic for ApplicationSearchView.xaml
    /// </summary>
    public partial class ApplicationSearchView : UserControl {
        public ApplicationSearchView(ApplicationSearchModel model) {
            InitializeComponent();
            this.DataContext = new ApplicationSearchViewModel(model);
        }
    }
}
