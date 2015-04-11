using System.Drawing;

namespace Orion.Zeta.Core.SearchMethods.Shared {
	public class Item : IItem {
		public Item(string value, Icon icon) {
			this.Value = value;
			this.Icon = icon;
		}

		public Item() {
		}

		public string Value { get; set; }

		public Icon Icon { get; set; }
	}
}