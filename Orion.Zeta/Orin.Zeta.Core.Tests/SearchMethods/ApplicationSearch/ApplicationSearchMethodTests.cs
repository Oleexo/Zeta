using NUnit.Framework;
using Orion.Zeta.Core.SearchMethods.ApplicationSearch;

namespace Orin.Zeta.Core.Tests.SearchMethods.ApplicationSearch {
	[TestFixture]
	public class ApplicationSearchMethodTests {
		private ApplicationSearchMethod _applicationSearchMethod;

		[SetUp]
		public void Setup() {
			this._applicationSearchMethod = new ApplicationSearchMethod();
		}

		[Test]
		public void GivenApplicationSearch_WhenIsMatchingWithGoodExpression_ThenShouldReturnTrue([Values("Cal", "Control Pa", "vs201", "test@test", "game-of year")] string expression) {
			var matching = this._applicationSearchMethod.IsMatching(expression);

			Assert.IsTrue(matching);
		}

		[Test]
		public void GivenApplicationSearch_WhenIsMachtingWithBadExpression_ThenShouldReturnFalse([Values("@application", "/home")] string expression) {
			var matching = this._applicationSearchMethod.IsMatching(expression);

			Assert.IsFalse(matching);			
		}
	}
}