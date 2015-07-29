namespace Orion.Zeta.Settings.Models {
	public class StyleModel {
		public StyleModel() {
			this.IsSlimDesign = false;
			this.Width = 500;
		}
		public double Width { get; set; }
		public bool IsSlimDesign { get; set; }
	}
}