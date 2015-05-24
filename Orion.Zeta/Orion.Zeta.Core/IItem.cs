using System.Drawing;

namespace Orion.Zeta.Core {
	public interface IItem {
		string Value { get; set; }

		string DisplayName { get; }

		Icon Icon { get; }

        /// <summary>
        /// Rank determined the probability of Item is the best. Lower the Rank is, higher the probability is strong. 0 is best propability.
        /// </summary>
        int Rank { get; }

		IExecute Execute { get; }

		bool IsValid();
	}
}