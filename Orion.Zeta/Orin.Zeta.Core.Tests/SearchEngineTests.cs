using Moq;
using NUnit.Framework;
using Orion.Zeta.Core;
using Orion.Zeta.Core.SearchMethods;

namespace Orin.Zeta.Core.Tests {
	[TestFixture]
	public class SearchEngineTests {
		private SearchEngine _searchEngine;
		private Mock<ISearchMethod> _searchMethodMock;
		private Mock<ISearchMethodAsync> _searchMethodAsyncMock;
		private const string ASearchExpression = "visual";

		[SetUp]
		public void Setup() {
			this._searchEngine = new SearchEngine();
			this._searchMethodMock = new Mock<ISearchMethod>();
			this._searchMethodAsyncMock = new Mock<ISearchMethodAsync>();
		}

		[Test]
		public async void GivenSearchEngineWithNoSearchMethod_WhenSearch_ThenShouldReturnEmptyList() {
			var items = await this._searchEngine.Search(ASearchExpression);

			Assert.IsEmpty(items);
		}

		[Test]
		public async void GivenSearchEngineWithOneMethod_WhenSearch_ThenShouldCallMethodSearchOnlyIfExpressionMatch() {
			this._searchEngine.RegisterMethod(this._searchMethodMock.Object);
			this._searchMethodMock.Setup(sm => sm.IsMatching(ASearchExpression)).Returns(true);

			await this._searchEngine.Search(ASearchExpression);

			this._searchMethodMock.Verify(sm => sm.Search(ASearchExpression), Times.Once);
		}
		[Test]
		public async void GivenSearchEngineWithOneMethod_WhenSearch_ThenShouldNotCallMethodSearchIfExpressionNotMatch() {
			this._searchEngine.RegisterMethod(this._searchMethodMock.Object);
			this._searchMethodMock.Setup(sm => sm.IsMatching(ASearchExpression)).Returns(false);

			await this._searchEngine.Search(ASearchExpression);

			this._searchMethodMock.Verify(sm => sm.Search(ASearchExpression), Times.Never);
		}

		[Test]
		public async void GivenSearchEngineWithOneAsyncMethod_WhenSearch_ThenShouldCallMethodSearchOnlyIfExpressionMatch() {
			this._searchEngine.RegisterMethod(this._searchMethodAsyncMock.Object);
			this._searchMethodAsyncMock.Setup(sm => sm.IsMatching(ASearchExpression)).Returns(true);

			await this._searchEngine.Search(ASearchExpression);

			this._searchMethodAsyncMock.Verify(sma => sma.SearchAsync(ASearchExpression));
		}

		[Test]
		public async void GivenSearchEngineWithOneAsyncMethod_WhenSearch_ThenShouldNotCallMethodSearchIfExpressionNotMatch() {
			this._searchEngine.RegisterMethod(this._searchMethodAsyncMock.Object);
			this._searchMethodAsyncMock.Setup(sm => sm.IsMatching(ASearchExpression)).Returns(false);

			await this._searchEngine.Search(ASearchExpression);

			this._searchMethodAsyncMock.Verify(sma => sma.SearchAsync(ASearchExpression), Times.Never);
		}
	}
}