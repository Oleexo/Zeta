using System.Drawing;

namespace Orion.Zeta.Core {
	public interface IItem {
		string Value { get; }

		Icon Icon { get; }
	}
}