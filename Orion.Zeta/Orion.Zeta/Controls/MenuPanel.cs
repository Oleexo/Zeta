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

		private Button _buttonLines;
		private ControlState _currentState;

		public MenuPanel() {
			this._currentState = ControlState.OpenState;
		}

		public override void OnApplyTemplate() {
			this._buttonLines = this.GetTemplateChild("PART_Button_lines") as Button;
			if (this._buttonLines != null) this._buttonLines.Click += this.OnButtonLinesOnClick;
			base.OnApplyTemplate();
		}

		private void OnButtonLinesOnClick(object sender, RoutedEventArgs routedEventArgs) {
			var state = (this._currentState == ControlState.OpenState ? ControlState.MinimizeState : ControlState.OpenState);
			this._currentState = state;
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
	}

	public class MenuPanelItem  {
		public string Header { get; set; }

		public ImageSource Icon { get; set; } 
	}
}
