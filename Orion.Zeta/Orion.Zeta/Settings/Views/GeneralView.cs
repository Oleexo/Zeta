using System;
using System.Windows;
using System.Windows.Controls;
using Orion.Zeta.Settings.Models;
using Orion.Zeta.Settings.ViewModels;

namespace Orion.Zeta.Settings.Views {
    /// <summary>
    /// Interaction logic for GeneralView.xaml
    /// </summary>
    public partial class GeneralView : UserControl {
        public GeneralView(GeneralModel model) {
            InitializeComponent();
            this.DataContext = new GeneralViewModel(model);
        }

        public void autoRefresh_OnIsCheckedChanged(object sender, EventArgs eventArgs) {
            if (this.autoRefresh.IsChecked.HasValue && this.autoRefresh.IsChecked.Value) {
                this.autoRefreshValue.Visibility = Visibility.Visible;
            }
            else {
                this.autoRefreshValue.Visibility = Visibility.Collapsed;
            }
        }
    }
}
