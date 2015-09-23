using System.Windows.Input;
using NHotkey;
using NHotkey.Wpf;

namespace Orion.Zeta {
	public static class HotKeyConfig {
		public static void Configuration(Zeta zeta) {
			try {
				HotkeyManager.Current.AddOrReplace("LaunchZeta", Key.Space, ModifierKeys.Alt, (sender, e) => {
					zeta.WakeUpApplication();
				});
			}
			catch (HotkeyAlreadyRegisteredException) {
#if !(DEBUG)
				MessageBox.Show("Sorry, Global hot key ALT + Space is already use");
				Current.Shutdown();
				return;
#endif
			}
		}
	}
}