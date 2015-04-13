using System.Linq;
using NUnit.Framework;
using Orion.Zeta.Core.SearchMethods.ExplorerSearch;

namespace Orin.Zeta.Core.Tests.SearchMethods {
	[TestFixture]
	public class ExplorerSearchMethodTests {
		private ExplorerSearchMethod _explorerSearchMethod;
		private const string CompletePathOfDirectory = "/";

		[SetUp]
		public void Setup() {
			this._explorerSearchMethod = new ExplorerSearchMethod();
		}

		[Test]
		public void GivenExplorer_WhenIsMatchingWithExpressionValid_ThenShouldReturnTrue([Values(
			@"/Users/Public",
			@"C/Users",
			@"/",
			@"/Work//Project.sln",
			@"C/Program Files (x68)/test.sln",
			@"c/TesT"
			)]string expression) {
			var result = this._explorerSearchMethod.IsMatching(expression);

			Assert.IsTrue(result);
		}

		[Test]
		public void GivenExplorer_WhenIsMatchingWithExpressionNotValid_ThenShouldReturnFalse([Values(
			@"ABC/Users/Admin",
			@"Users"
			)]string expression) {
			var result = this._explorerSearchMethod.IsMatching(expression);

			Assert.IsFalse(result);
		}

		[Test]
		public void GivenExplorer_WhenSearch_ThenShouldReturnPossibiltyWhoStartWithExpression([Values(
			@"/Users/Pub",
			@"c/Prog",
			@"/Prog",
			@"~/Des",
			@"~/Down"
			)]string expression) {
			var results = this._explorerSearchMethod.Search(expression);

			Assert.IsNotEmpty(results);
			Assert.IsTrue(results.All(r => r.Value.StartsWith(expression)));
		}

		[Test]
		public void GivenExplorer_WhenSearchWithCompleteDirectoryPath_ThenShouldNotReturnResult() {
			var results = this._explorerSearchMethod.Search(CompletePathOfDirectory);

			Assert.IsEmpty(results);
		}
	}
}