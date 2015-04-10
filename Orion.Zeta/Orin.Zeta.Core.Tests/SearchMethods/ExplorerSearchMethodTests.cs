using NUnit.Framework;
using Orion.Zeta.Core.SearchMethods;

namespace Orin.Zeta.Core.Tests.SearchMethods {
	[TestFixture]
	public class ExplorerSearchMethodTests {
		private ExplorerSearchMethod _explorerSearchMethod;
		private const string ExpressionBeginSlash = "/Users";

		[SetUp]
		public void Setup() {
			this._explorerSearchMethod = new ExplorerSearchMethod();
		}

		[Test]
		public void GivenExplorer_WhenIsMatchingWithExpressionWhoBeginWithSlash_ThenShouldReturnTrue([Values(
			@"/Users/Admin",
			@"C/Users",
			@"/"
			)]string expression) {
			var result = this._explorerSearchMethod.IsMatching(expression);

			Assert.IsTrue(result);
		}
	}
}