using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Orion.Zeta.Core.SearchMethods.Shared {
	public interface IFileSystemSearch {
		IEnumerable<string> GetFiles(string path, string pattern, SearchOption searchOption = SearchOption.TopDirectoryOnly);
		IEnumerable<string> GetDirectories(string path, string pattern, SearchOption searchOption = SearchOption.TopDirectoryOnly);
		bool DirectoryExists(string path);

		string GetFilename(string path);

		Icon GetIcon(string path);
	}
}