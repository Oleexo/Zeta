using System.IO;
using NUnit.Framework;
using Orion.Zeta.Methods.Dev.Shared;
using Orion.Zeta.Methods.Dev.Shared.Implementations;

namespace Orin.Zeta.Core.Tests.SearchMethods.Shared {
	[TestFixture]
	public class FileSystemSearchTests {
		private FileSystemSearch _fileSystemSearch;
		private const string FolderPath = @"C:\Windows";
		private const string PatternDirectory = @"add*";
		private const string PatternFile = @"hh.*";

		[SetUp]
		public void Setup() {
			this._fileSystemSearch = new FileSystemSearch();
		}

		[Test]
		public void GivenInstance_WhenGetFiles_ThenShouldReturnRealPathOfFileSystem() {
			var files = this._fileSystemSearch.GetFiles(FolderPath, PatternFile);

			Assert.IsNotEmpty(files);
			foreach (var path in files) {
				Assert.IsTrue(File.Exists(path));				
			}
		}

		[Test]
		public void GivenInstance_WhenGetDirectories_ThenShouldReturnRealPathOfFileSystem() {
			var directories = this._fileSystemSearch.GetDirectories(FolderPath, PatternDirectory);

			Assert.IsNotEmpty(directories);
			foreach (var path in directories) {
				Assert.IsTrue(Directory.Exists(path));
			}
		}
	}
}