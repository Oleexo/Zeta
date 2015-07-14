using System.Drawing;

namespace Orion.Zeta.Methods.Dev.Shared {
	public class Item : IItem {
		public Item(string value, Icon icon) {
			this.Value = value;
			this.Icon = icon;
		}

		public Item() {
		}

		public string Value { get; set; }

		public string DisplayName { get; set; }

		public Icon Icon { get; set; }

		public int Rank { get; set; }

		public IExecute Execute { get; set; }

		public bool IsValid() {
			return this.Execute != null;
		}

		public Item Clone() {
			return new Item {
				Value = this.Value,
				DisplayName = this.DisplayName,
				Execute = this.Execute,
				Icon = this.Icon	
			};
		}
	}
}