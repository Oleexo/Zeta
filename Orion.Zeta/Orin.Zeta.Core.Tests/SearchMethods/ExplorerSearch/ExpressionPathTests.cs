using System.Collections.Generic;
using System.IO;
using System.Linq;
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
		private const string ExpressionCompletePathOfDirectory = "/";
		private const string ExpressionCompletePathOfDirectoryParentDirectory = @"C:\";
		private const string ExpressionCompletePathOfDirectoryPattern = "*";

		private IEnumerable<string> _listOfFiles;
		private IEnumerable<string> _listOfDirectories;
		private const string FileNameLong = "/file12";
		private const string FileNameShort = "/file1";
		private const string FileNameAlphabeticFirst = "/fileABC";
		private const string FileNameAlphabeticSecond = "/fileDAA";

		[SetUp]
		public void Setup() {
			this._listOfFiles = new List<string> {
				@"C:\File1",
				@"C:\File2",
				@"C:\File3",
				@"C:\File4",
				@"C:\File5",
				@"C:\file6",
				@"C:\File12",
				@"C:\FileABC",
				@"C:\FileDAA",
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
			this._iFileSystemSearchMock.Setup(fss => fss.GetFiles(ExpressionCompletePathOfDirectoryParentDirectory, ExpressionCompletePathOfDirectoryPattern, SearchOption.TopDirectoryOnly))
				.Returns(this._listOfFiles);
			this._iFileSystemSearchMock.Setup(fss => fss.GetDirectories(ExpressionCompletePathOfDirectoryParentDirectory, ExpressionCompletePathOfDirectoryPattern, SearchOption.TopDirectoryOnly))
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

		[Test]
		public void GivenExpressionPathWithCompleteDirectoryPath_WhenFindPossibilities_ThenShouldReturnResult() {
			var expressionPath = new ExpressionPath(ExpressionCompletePathOfDirectory, this._iFileSystemSearchMock.Object);

			var possibilities = expressionPath.FindPossibilities();

			Assert.IsNotEmpty(possibilities);
		}

		[Test]
		public void GivenExpressionPathWithExpression_WhenFindPossibilities_ThenRankOfShortFilenameIsLowerThanOther() {
			var expressionPath = new ExpressionPath(Expression, this._iFileSystemSearchMock.Object);

			var possibilities = expressionPath.FindPossibilities();

			var items = possibilities.Select(p => p.ToItem());
			var rankShortFileName = items.First(i => i.Value == FileNameShort).Rank;
			var rankLongFileName = items.First(i => i.Value == FileNameLong).Rank;
			Assert.IsTrue( rankShortFileName < rankLongFileName);
		}

		[Test]
		public void GivenExpressionPathWithExpression_WhenFindPossibilities_ThenRankOfFilenameDependOfAlphabeticalOrder() {
			var expressionPath = new ExpressionPath(Expression, this._iFileSystemSearchMock.Object);

			var possibilities = expressionPath.FindPossibilities();

			var items = possibilities.Select(p => p.ToItem());
			var rankFirstFileName = items.First(i => i.Value == FileNameAlphabeticFirst).Rank;
			var rankSecondFileName = items.First(i => i.Value == FileNameAlphabeticSecond).Rank;
			Assert.IsTrue(rankFirstFileName < rankSecondFileName);
		}
	}
}