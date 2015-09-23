using System.Linq;
using NUnit.Framework;
using Orion.Zeta.Methods.WebSearch;

namespace Orin.Zeta.Core.Tests.SearchMethods.WebSearch {
	[TestFixture]
	public class WebSearchMethodTest {
		private WebSearchMethod _webSearchMethod;

		[SetUp]
		public void Setup() {
			_webSearchMethod = new WebSearchMethod();
			_webSearchMethod.Initialisation();
		}

		[Test]
		public void GivenWebSearchMethod_WhenIsMatchingWithGoodExpression_ThenShouldReturnTrue([Values("@test", "@0101", "@tset@")] string expression) {
			var matching = this._webSearchMethod.IsMatching(expression);

			Assert.IsTrue(matching);
		}

		[Test]
		public void GivenWebSearchMethod_WhenIsMatchingWithBadExpression_ThenShouldReturnFalse([Values("test", ":test", "t@set", "test@", "test@test@", "@")] string expression) {
			var matching = this._webSearchMethod.IsMatching(expression);

			Assert.IsFalse(matching);
		}

		[Test]
		public void GivenWebSearchMethod_WhenSearch_ThenShouldReturnOneItem() {
			var results = this._webSearchMethod.Search("@Zeta");

			Assert.AreEqual(1, results.Count());
		}

		[Test]
		public void GivenWebSearchMethod_WhenSearch_ThenShouldHaveAnIcon() {
			var results = this._webSearchMethod.Search("@Zeta");

			var item = results.FirstOrDefault();
			Assert.IsNotNull(item.Icon);
		}
	}
}