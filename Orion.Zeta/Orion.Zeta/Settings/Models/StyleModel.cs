namespace Orion.Zeta.Settings.Models {
	public class StyleModel {
		public StyleModel() {
			this.IsSlimDesign = false;
			this.Width = 500;
			this.IsAlwaysOnTop = true;
			this.IsHideWhenLostFocus = true;
		}
		public double Width { get; set; }
		public bool IsSlimDesign { get; set; }
		public bool IsHideWhenLostFocus { get; set; }
		public bool IsAlwaysOnTop { get; set; }

	}
}