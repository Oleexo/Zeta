using System;
using System.Drawing;
using Orion.Zeta.Core.SearchMethods.Shared;

namespace Orion.Zeta.Core.SearchMethods.ExplorerSearch {
	public class PossibilityPath : IComparable<PossibilityPath> {

		public enum PathType {
			Directory,
			File
		}

		public PossibilityPath(string path, string baseExpression, PathType type) {
			this._baseExpression = baseExpression;
			this._path = path;
			this._type = type;
		}

		public int Rank {
			set { this._rank = value; }
		}

		private readonly string _baseExpression;

		private readonly string _path;
		private readonly PathType _type;
		private const string ExplorerExe = "explorer.exe";
		private const int MultiplierRank = 20;
		private int _rank;

		public IItem ToItem() {
			var item = new Item {
				Value = PathHelper.ConvertToPseudoPath(this._path, this._baseExpression),
				DisplayName = PathHelper.GetPattern(this._path),
				Rank = this._rank,
				Execute = new Execute {
					Program = this._type == PathType.Directory ? ExplorerExe : this._path,
					Parameters = this._type == PathType.Directory ? this._path : string.Empty
				}
			};
			Icon icon;
			try {
				icon = IconHelper.GetIcon(this._path);
			}
			catch (Exception) {
				icon = SystemIcons.WinLogo;
			}
			item.Icon = icon;
			return item;
		}

		public int CompareTo(PossibilityPath other) {
			return String.Compare(this._path, other._path, StringComparison.OrdinalIgnoreCase);
		}
	}
}