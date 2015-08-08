using System.IO;
using IWshRuntimeLibrary;

namespace Orion.Zeta.Core {
	public class Shortcut {
		private readonly IWshShortcut _shortcut;

		private Shortcut(IWshShortcut shortcut) {
			this._shortcut = shortcut;
		}



		public static Shortcut Create(string shortcutName, string shortcutPath, string targetFileLocation, string description = null, string iconLocation = null) {
			string shortcutLocation = Path.Combine(shortcutPath, shortcutName + ".lnk");
			WshShell shell = new WshShell();
			IWshShortcut shortcut = (IWshShortcut) shell.CreateShortcut(shortcutLocation);

			shortcut.Description = description ?? string.Empty;
			if (iconLocation != null) {
				shortcut.IconLocation = iconLocation;
			}
			shortcut.TargetPath = targetFileLocation;
			shortcut.Save();
			return new Shortcut(shortcut);
		}
	}
}