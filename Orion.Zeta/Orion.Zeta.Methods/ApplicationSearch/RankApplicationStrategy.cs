using System;
using System.Linq;
using Orion.Zeta.Methods.Dev.Shared;

namespace Orion.Zeta.Methods.ApplicationSearch {
	public class RankApplicationStrategy {
		public int GetRank(Item item, string expression) {
			if (item.DisplayName.Equals(expression, StringComparison.OrdinalIgnoreCase)) {
				return 0;
			}
			return this.RankBasedOnUpperCase(expression, item.DisplayName);
		}

		private int RankBasedOnUpperCase(string expression, string itemName) {
			var upperChar = this.ExtractUpperChar(itemName);
			var numberCharMatch = 0;
			var numberUpperCaseMatch = 0;
			var posItemName = 0;
			var posExpression = 0;
			while (posExpression < expression.Length && posItemName < itemName.Length) {
				if (Char.ToUpperInvariant(expression[posExpression]) == Char.ToUpperInvariant(itemName[posItemName])) {
					if (Char.IsUpper(itemName[posItemName]))
						++numberUpperCaseMatch;
					else {
						if (upperChar.Length > numberUpperCaseMatch && upperChar[numberUpperCaseMatch] == Char.ToUpperInvariant(expression[posExpression])) {
							++posItemName;
							continue;
						}
					}
					++numberCharMatch;
					++posExpression;
				}
				++posItemName;
			}
			var upperCaseMultiplier = 3 - (upperChar.Length - numberUpperCaseMatch);
			var rank = 100 + (itemName.Length - numberCharMatch) * 2 - numberUpperCaseMatch * 10 * upperCaseMultiplier;
			return rank < 0 ? 0 : rank;
		}

		private string ExtractUpperChar(string expression) {
			return String.Concat(expression.Where(Char.IsUpper));
		}
	}
}