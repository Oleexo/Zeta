using System.Windows;
using System.Windows.Controls;

namespace Orion.Zeta.Controls {

	public class TextSuggestBox : TextBox {
		static TextSuggestBox() {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TextSuggestBox), new FrameworkPropertyMetadata(typeof(TextSuggestBox)));
			SuggestionProperty = DependencyProperty.Register("Suggestion", typeof (string), typeof (TextSuggestBox));
		}

		public static DependencyProperty SuggestionProperty;

		public string Suggestion {
			get { return (string) this.GetValue(SuggestionProperty); }
			set { this.SetValue(SuggestionProperty, value);}
		}
	}
}
