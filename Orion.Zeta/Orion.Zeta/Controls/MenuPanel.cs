using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Orion.Zeta.Controls {

    public class MenuPanel : Control {
        private enum ControlState {
            OpenState,
            MinimizeState
        }
        static MenuPanel() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuPanel), new FrameworkPropertyMetadata(typeof(MenuPanel)));
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate",
            typeof(DataTemplate), typeof(MenuPanel), new UIPropertyMetadata(null));

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource",
            typeof(IEnumerable), typeof(MenuPanel), new FrameworkPropertyMetadata((IEnumerable) null));

        public static readonly DependencyProperty IsMinimizeProperty = DependencyProperty.Register("IsMinimize",
            typeof(bool), typeof(MenuPanel), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem",
            typeof(object), typeof(MenuPanel), new FrameworkPropertyMetadata(null));

        private Button _buttonLines;
        private ControlState _currentState;
        private ListBox _listMenuItem;

        public MenuPanel() {
            this.CurrentState = ControlState.OpenState;
        }

        public bool IsMinimize {
            get { return (bool) this.GetValue(IsMinimizeProperty); }
            private set { this.SetValue(IsMinimizeProperty, value); }
        }

        public override void OnApplyTemplate() {
            this._buttonLines = this.GetTemplateChild("PART_Button_lines") as Button;
            this._listMenuItem = this.GetTemplateChild("PART_List_panel") as ListBox;
            if (this._buttonLines != null) this._buttonLines.Click += this.OnButtonLinesOnClick;
            if (this._listMenuItem != null) this._listMenuItem.SelectionChanged += this.OnSelectionChanged;
            base.OnApplyTemplate();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            this.SelectedItem = this._listMenuItem.SelectedItem;
        }

        private void OnButtonLinesOnClick(object sender, RoutedEventArgs routedEventArgs) {
            var state = (this.CurrentState == ControlState.OpenState ? ControlState.MinimizeState : ControlState.OpenState);
            this.CurrentState = state;
            VisualStateManager.GoToState(this, state.ToString(), true);
        }

        public DataTemplate ItemTemplate {
            get { return (DataTemplate) this.GetValue(ItemTemplateProperty); }
            set { this.SetValue(ItemTemplateProperty, value); }
        }

        public IEnumerable ItemsSource {
            get { return (IEnumerable) this.GetValue(ItemsSourceProperty); }
            set { this.SetValue(ItemsSourceProperty, value); }
        }

        private ControlState CurrentState {
            get { return this._currentState; }
            set {
                this._currentState = value;
                if (this.IsMinimize) {
                    if (value == ControlState.OpenState)
                        this.IsMinimize = false;
                } else {
                    if (value == ControlState.MinimizeState)
                        this.IsMinimize = true;
                }
            }
        }

        public object SelectedItem {
            get { return this.GetValue(SelectedItemProperty); }
            set { this.SetValue(SelectedItemProperty, value); }
        }
    }

    public class MenuPanelItem {
        public string Header { get; set; }

        public ImageSource Image { get; set; }

        public Canvas Icon { get; set; }

        public UserControl Control { get; set; }
    }
}
