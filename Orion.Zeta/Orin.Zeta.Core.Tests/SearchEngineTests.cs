using Moq;
using NUnit.Framework;
using Orion.Zeta.Core;
using Orion.Zeta.Core.SearchMethods;

namespace Orin.Zeta.Core.Tests {
	[TestFixture]
	public class SearchEngineTests {
		private SearchEngine _searchEngine;
		private Mock<ISearchMethod> _searchMethodMock;
		private const string ASearchExpression = "visual";

		[SetUp]
		public void Setup() {
			this._searchEngine = new SearchEngine();
			this._searchMethodMock = new Mock<ISearchMethod>();
		}

		[Test]
		public void GivenSearchEngineWithNoSearchMethod_WhenSearch_ThenShouldReturnEmptyList() {
			var items = this._searchEngine.Search(ASearchExpression);

			Assert.IsEmpty(items);
		}

		[Test]
		public void GivenSearchEngineWithOneMethod_WhenSearch_ThenShouldCallMethodSearchOnlyIfExpressionMatch() {
			this._searchEngine.RegisterMethod(this._searchMethodMock.Object);
			this._searchMethodMock.Setup(sm => sm.IsMatching(ASearchExpression)).Returns(true);

			var items = this._searchEngine.Search(ASearchExpression);

			this._searchMethodMock.Verify(sm => sm.Search(ASearchExpression), Times.Once);
		}
		[Test]
		public void GivenSearchEngineWithOneMethod_WhenSearch_ThenShouldNotCallMethodSearchIfExpressionNotMatch() {
			this._searchEngine.RegisterMethod(this._searchMethodMock.Object);
			this._searchMethodMock.Setup(sm => sm.IsMatching(ASearchExpression)).Returns(false);

			var items = this._searchEngine.Search(ASearchExpression);

			this._searchMethodMock.Verify(sm => sm.Search(ASearchExpression), Times.Never);
		}
	}
}