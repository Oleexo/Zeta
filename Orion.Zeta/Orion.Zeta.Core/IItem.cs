using System.Drawing;

namespace Orion.Zeta.Core {
	public interface IItem {
		string Value { get; set; }

		string DisplayName { get; }

		Icon Icon { get; }

		IExecute Execute { get; }
		bool IsValid();
	}
}