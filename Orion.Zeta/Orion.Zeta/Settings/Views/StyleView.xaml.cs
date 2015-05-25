using System.Windows.Controls;
using Orion.Zeta.Settings.Models;

namespace Orion.Zeta.Settings.Views {
    /// <summary>
    /// Interaction logic for StyleView.xaml
    /// </summary>
    public partial class StyleView : UserControl {
        private readonly StyleModel model;

        public StyleView(StyleModel model) {
            this.model = model;
            InitializeComponent();
        }
    }
}
