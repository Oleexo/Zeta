using System.Collections.Generic;
using System.IO;
using Moq;
using NUnit.Framework;
using Orion.Zeta.Core.SearchMethods.ExplorerSearch;
using Orion.Zeta.Core.SearchMethods.Shared;

namespace Orin.Zeta.Core.Tests.SearchMethods.ExplorerSearch {
	[TestFixture]
	public class ExpressionPathTests {
		private Mock<IFileSystemSearch> _iFileSystemSearchMock;
		private const string Expression = @"/fi";
		private const string BadExpression = @"/fake";
		private const string ExpressionParentDirectory = @"C:\";
		private const string ExpressionPattern = @"fi*";
		private IEnumerable<string> _listOfFiles;
		private IEnumerable<string> _listOfDirectories;

		[SetUp]
		public void Setup() {
			this._listOfFiles = new List<string> {
				@"C:\File1",
				@"C:\File2",
				@"C:\File3",
				@"C:\File4",
				@"C:\File5",
				@"C:\file6"
			};
			this._listOfDirectories = new List<string> {
				@"C:\Directory1",
				@"C:\Directory2",
				@"C:\Directory3",
				@"C:\Directory4",
				@"C:\Directory5",
				@"C:\directory6",
			};
			this._iFileSystemSearchMock = new Mock<IFileSystemSearch>();
			this._iFileSystemSearchMock.Setup(fss => fss.GetFiles(ExpressionParentDirectory, ExpressionPattern, SearchOption.TopDirectoryOnly))
				.Returns(this._listOfFiles);
			this._iFileSystemSearchMock.Setup(fss => fss.GetDirectories(ExpressionParentDirectory, ExpressionPattern, SearchOption.TopDirectoryOnly))
				.Returns(this._listOfDirectories);
		}

		[Test]
		public void GivenExpressionPathWithExpression_WhenFindPossibilities_ThenShouldReturnNotEmptyListOfPossibilities() {
			var expressionPath = new ExpressionPath(Expression, this._iFileSystemSearchMock.Object);

			var possibilities = expressionPath.FindPossibilities();

			Assert.IsNotEmpty(possibilities);
		}

		[Test]
		public void GivenExpressionPathWithExpression_WhenFindPossibilities_ThenShouldReturnPossibilitiesFromFileSystemSearch() {
			var expressionPath = new ExpressionPath(Expression, this._iFileSystemSearchMock.Object);

			var possibilities = expressionPath.FindPossibilities();

			this._iFileSystemSearchMock.Verify(fss => fss.GetFiles(ExpressionParentDirectory, ExpressionPattern, SearchOption.TopDirectoryOnly));
			this._iFileSystemSearchMock.Verify(fss => fss.GetDirectories(ExpressionParentDirectory, ExpressionPattern, SearchOption.TopDirectoryOnly));
		}

		[Test]
		public void GivenExpressionPathWithNoResultExpression_WhenFindPossibilities_ThenShouldNotReturnPossibilities() {
			var expressionPath = new ExpressionPath(BadExpression, this._iFileSystemSearchMock.Object);

			var possibilities = expressionPath.FindPossibilities();

			Assert.IsEmpty(possibilities);			
		}
	}
}