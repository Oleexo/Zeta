using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Orion.Zeta.Core.SearchMethods.Shared {
	public class IconHelper {
		[DllImport("shell32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool DestroyIcon(IntPtr hIcon);

		public const uint SHGFI_ICON = 0x000000100;
		public const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
		public const uint SHGFI_OPENICON = 0x000000002;
		public const uint SHGFI_SMALLICON = 0x000000001;
		public const uint SHGFI_LARGEICON = 0x000000000;
		public const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;

		public enum FolderType {
			Closed,
			Open
		}

		public enum IconSize {
			Large,
			Small
		}


		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct SHFILEINFO {
			public IntPtr hIcon;
			public int iIcon;
			public uint dwAttributes;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szDisplayName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		};

		private static Icon GetFolderIcon(IconSize size, FolderType folderType) {
			// Need to add size check, although errors generated at present!    
			uint flags = SHGFI_ICON | SHGFI_USEFILEATTRIBUTES;

			if (FolderType.Open == folderType) {
				flags += SHGFI_OPENICON;
			}
			if (IconSize.Small == size) {
				flags += SHGFI_SMALLICON;
			}
			else {
				flags += SHGFI_LARGEICON;
			}
			// Get the folder icon    
			var shfi = new SHFILEINFO();

			var res = SHGetFileInfo(@"C:\Windows",
				FILE_ATTRIBUTE_DIRECTORY,
				ref shfi,
				(uint) Marshal.SizeOf(shfi),
				flags);

			if (res == IntPtr.Zero)
				throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());

			// Load the icon from an HICON handle  
			Icon.FromHandle(shfi.hIcon);

			// Now clone the icon, so that it can be successfully stored in an ImageList
			var icon = (Icon) Icon.FromHandle(shfi.hIcon).Clone();

			DestroyIcon(shfi.hIcon);        // Cleanup    

			return icon;
		}

		public static Icon GetIcon(string path, IconSize size = IconSize.Large) {
			var shinfo = new SHFILEINFO();
			IntPtr res;
			switch (size) {
				case IconSize.Large:
					res = SHGetFileInfo(path, 0, ref shinfo, (uint) Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);
					break;
				case IconSize.Small:
					res = SHGetFileInfo(path, 0, ref shinfo, (uint) Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON);
					break;
				default:
					throw new ArgumentOutOfRangeException("size");
			}
			if (res == IntPtr.Zero)
				throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
			Icon icon = null;
			try {
				// TODO find why this throw
				icon = Icon.FromHandle(shinfo.hIcon);
			}
			catch (Exception e) {
				Console.WriteLine(e.Message);
			}

			return icon;
		}
	}
}