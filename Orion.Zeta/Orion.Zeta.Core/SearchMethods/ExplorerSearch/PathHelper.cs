using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Orion.Zeta.Core.SearchMethods.ExplorerSearch {
	public static class PathHelper {
		public static string GetPattern(string path) {
			return path.Substring(path.LastIndexOf("\\", StringComparison.Ordinal) + 1);
		}

		public static string GetParentDirectory(string path) {
			return path.Substring(0, path.LastIndexOf("\\", StringComparison.Ordinal) + 1);
		}

		public static string ConvertToPseudoPath(string winPath, string basePath) {
			string path;
			if (basePath.StartsWith("/")) {
				path = winPath.Substring(2).Replace("\\", "/");
			}
			else if (basePath.StartsWith("~/")) {
				path = "~" + winPath.Replace(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "").Replace("\\", "/");
			}
			else {
				path = basePath.Substring(0, 1) + winPath.Substring(2).Replace("\\", "/");
			}
			return basePath + path.Substring(basePath.Length);
		}

		public static IEnumerable<string> ConvertToWinPath(string expression) {
			var paths = new List<string>();
			if (expression.StartsWith("/")) {
				var drives = DriveInfo.GetDrives();
				foreach (var driveInfo in drives.Where(d => d.DriveType == DriveType.Fixed || d.DriveType == DriveType.Removable)) {
					var path = driveInfo.Name;
					path += expression.Substring(1).Replace("/", "\\");
					paths.Add(path);
				}
			}
			else if (expression.StartsWith("~/")) {
				var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
				path += expression.Substring(1).Replace("/", "\\");
				paths.Add(path);
			}
			else {
				var path = expression.Substring(0, 1).ToUpper() + ":" + expression.Substring(1).Replace("/", "\\");
				paths.Add(path);
			}
			return paths;

		}

	}

}