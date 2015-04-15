using System.Collections.Generic;
using System.IO;

namespace Orion.Zeta.Core.SearchMethods.Shared {
	public interface IFileSystemSearch {
		IEnumerable<string> GetFiles(string path, string pattern, SearchOption searchOption = SearchOption.TopDirectoryOnly);
		IEnumerable<string> GetDirectories(string path, string pattern, SearchOption searchOption = SearchOption.TopDirectoryOnly);
	}
}