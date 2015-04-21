using System.Linq;
using NUnit.Framework;
using Orion.Zeta.Core.SearchMethods.ExplorerSearch;

namespace Orin.Zeta.Core.Tests.SearchMethods.ExplorerSearch {
	/// <summary>
	/// This test work only on Windows OS
	/// </summary>
	[TestFixture]
	public class ExplorerSearchMethodTests {
		private ExplorerSearchMethod _explorerSearchMethod;

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
			@"c/TesT",
			@"/université Laval"
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
			@"~/Down",
			@"/use",
			@"/Windows/hh.")]string expression) {
			var results = this._explorerSearchMethod.Search(expression);

			Assert.IsNotEmpty(results);
			Assert.IsTrue(results.All(r => r.Value.StartsWith(expression)));
		}

		[Test]
		public async void GivenExplorer_WhenSearchAsync_ThenShouldReturnPossibiltyWhoStartWithExpression([Values(
			@"/Users/Pub",
			@"c/Prog",
			@"/Prog",
			@"~/Des",
			@"~/Down",
			@"/use",
			@"/Windows/hh.")]string expression) {
			var results = await this._explorerSearchMethod.SearchAsync(expression);

			Assert.IsNotEmpty(results);
			Assert.IsTrue(results.All(r => r.Value.StartsWith(expression)));
		}
	}
}