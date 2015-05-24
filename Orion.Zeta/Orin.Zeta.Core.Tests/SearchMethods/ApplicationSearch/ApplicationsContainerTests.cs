using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using Orion.Zeta.Core.SearchMethods.ApplicationSearch;
using Orion.Zeta.Core.SearchMethods.Shared;

namespace Orin.Zeta.Core.Tests.SearchMethods.ApplicationSearch {
	[TestFixture]
	public class ApplicationsContainerTests {
		private const string Path = "c:\\";
		private const string BadPath = "bad Path";
		private IEnumerable<string> _patterns;
		private Mock<IFileSystemSearch> _iFileSystemSearchMock;
		private ApplicationsContainer _applicationsContainer;
		private List<string> _files;
		private const string Expression = "vs201";
		private const string EmptyExpression = "";
		private const string Pattern = "*.exe";
		private const string ApplicationName = "Visual Studio 2015";
		private const string ApplicationPath = @"C:\Visual Studio 2015.exe";
		private const string ExpressionPerfectMatch = ApplicationName;
		private const string OtherApplicationPath = @"C:\Visio 2013.exe";
		private const string OtherApplicationName = "Visio 2013";


		[SetUp]
		public void Setup() {
			this._files = new List<string> {
				ApplicationPath,
				OtherApplicationPath,
				@"C:\Calculator.exe",
				@"C:\Paint.exe",
				@"C:\Word 2013.exe"
			};
			this._iFileSystemSearchMock = new Mock<IFileSystemSearch>();
			this._iFileSystemSearchMock.Setup(fss => fss.DirectoryExists(Path)).Returns(true);
			this._iFileSystemSearchMock.Setup(fss => fss.DirectoryExists(BadPath)).Returns(false);
			this._iFileSystemSearchMock.Setup(fss => fss.GetFiles(Path, It.IsAny<string>(), It.IsAny<SearchOption>())).Returns(this._files);
			this._iFileSystemSearchMock.Setup(fss => fss.GetFilename(It.IsAny<string>())).Returns(string.Empty);
			this._iFileSystemSearchMock.Setup(fss => fss.GetFilename(ApplicationPath)).Returns(ApplicationName);
			this._iFileSystemSearchMock.Setup(fss => fss.GetFilename(OtherApplicationPath)).Returns(OtherApplicationName);
			this._patterns = new List<string> {
				Pattern
			};
			this._applicationsContainer = new ApplicationsContainer(Path, this._patterns, this._iFileSystemSearchMock.Object);
		}

		[Test]
		public void GivenNewApplicationContainer_WhenCreate_ThenShouldVerifiyPath() {
			new ApplicationsContainer(Path, this._patterns, this._iFileSystemSearchMock.Object);

			this._iFileSystemSearchMock.Setup(fss => fss.DirectoryExists(Path));
		}

		[Test]
		[ExpectedException(typeof(DirectoryNotFoundException))]
		public void GivenNewApplicationContainer_WhenCreateWithBadPath_ThenShouldThrowException() {
			new ApplicationsContainer(BadPath, this._patterns, this._iFileSystemSearchMock.Object);
		}

		[Test]
		public void GivenApplicationContainerWithValidInformation_WhenSearch_ThenShouldUseFileSystemSearch() {
			this._applicationsContainer.Search(Expression);

			this._iFileSystemSearchMock.Verify(fss => fss.GetFiles(Path, It.IsAny<string>(), SearchOption.AllDirectories));
		}

		[Test]
		public void GivenApplicationContainerWithValidInformation_WhenSearch_ThenShouldReturnItemMatchWithExpression() {
			var results = this._applicationsContainer.Search(Expression);

			Assert.IsNotEmpty(results);
			var result = results.First();
			Assert.AreEqual(result.Value, ApplicationPath);
		}

		[Test]
		public void GivenApplicationContainerWithValidInformation_WhenSearchWithEmptyExpression_ThenShouldNotReturnResult() {
			var results = this._applicationsContainer.Search(EmptyExpression);

			Assert.IsEmpty(results);
		}

		[Test]
		public void GivenApplicationContainerWithValidInformation_WhenSearchWithExpression_ThenPerfectMatchItemShouldHaveRankOfZero() {
			var results = this._applicationsContainer.Search(ExpressionPerfectMatch);

			var result = results.FirstOrDefault();
			Assert.AreEqual(0, result.Rank);
		}

		[Test]
		public void GivenApplicationContainerWithValidInformation_WhenSearchWithExpression_ThenNoPerfectMatchItemShouldNotHaveRankOfZero() {
			var results = this._applicationsContainer.Search(Expression);

			var result = results.FirstOrDefault();
			Assert.AreNotEqual(0, result.Rank);
		}

		[Test]
		public void GivenApplicationContainerWithValidInformation_WhenSearchWithExpression_ThenMatchBetterWithUpperCase() {
			var results = this._applicationsContainer.Search(Expression);

			var applicationFirstRank = results.First(r => r.DisplayName == ApplicationName).Rank;
			var applicationSecondRank = results.First(r => r.DisplayName == OtherApplicationName).Rank;
			Assert.IsTrue(applicationFirstRank < applicationSecondRank);
		}
	}
}