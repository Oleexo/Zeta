using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Orion.Zeta.Core.SearchMethods.Shared;

namespace Orion.Zeta.Core.SearchMethods.ExplorerSearch {
	internal class ExpressionPath {
		private enum PathType {
			Directory,
			File
		}
		private readonly string _expression;
		private readonly List<Tuple<PathType, string>> _possibilties;

		public ExpressionPath(string expression) {
			this._expression = expression;
			this._possibilties = new List<Tuple<PathType, string>>();
		}

		public void FindPossibilities() {
			this.GeneratePossibilities(this.GeneratePaths());
		}

		private void GeneratePossibilities(IEnumerable<string> paths) {
			foreach (var path in paths) {
				var parentDirectoryPath = this.GetParentDirectory(path);
				var pattern = this.GetPattern(path);
				if (String.IsNullOrEmpty(pattern)) {
					continue;
				}
				pattern += "*";
				var files = Directory.EnumerateFiles(parentDirectoryPath, pattern, SearchOption.TopDirectoryOnly);
				foreach (var file in files) {
					this._possibilties.Add(new Tuple<PathType, string>(PathType.File, file));
				}
				var directories = Directory.GetDirectories(parentDirectoryPath, pattern, SearchOption.TopDirectoryOnly);
				foreach (var directory in directories) {
					/*
					 *  Path because /Example/Zeta/Zeta.
					 *  Match with /Example/Zeta/Zeta
					 *  if /Example/Zeta contains folder name Zeta
					 * */
					if (directory.Length >= path.Length)
						this._possibilties.Add(new Tuple<PathType, string>(PathType.Directory, directory));
				}
			}
		}

		private string GetPattern(string path) {
			return path.Substring(path.LastIndexOf("\\", StringComparison.Ordinal) + 1);
		}

		private string GetParentDirectory(string path) {
			return path.Substring(0, path.LastIndexOf("\\", StringComparison.Ordinal) + 1);
		}

		private IEnumerable<string> GeneratePaths() {
			var paths = new List<string>();
			var expression = this._expression;
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

		public IEnumerable<IItem> GetItems() {
			var results = new List<IItem>();
			foreach (var possibilty in this._possibilties) {
				var item = new Item {
					Value = this.ConvertToPseudoPath(possibilty.Item2)
				};
				Icon icon;
				try {
					icon = Icon.ExtractAssociatedIcon(possibilty.Item2);
				}
				catch (Exception) {
					icon = SystemIcons.WinLogo;
				}
				item.Icon = icon;
				results.Add(item);
			}
			return results;
		}

		private string ConvertToPseudoPath(string winPath) {
			string path;
			if (this._expression.StartsWith("/")) {
				path = winPath.Substring(2).Replace("\\", "/");
			}
			else if (this._expression.StartsWith("~/")) {
				path = "~" + winPath.Replace(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "").Replace("\\", "/");
			}
			else {
				path = this._expression.Substring(0, 1) + winPath.Substring(2).Replace("\\", "/");
			}
			return this._expression + path.Substring(this._expression.Length);
		}
	}
}