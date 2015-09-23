using Hardcodet.Wpf.TaskbarNotification;
using Orion.Zeta.ViewModels;

namespace Orion.Zeta {
	public static class NotifyIconConfig {
		private static NotifyIconViewModel _notifyIconViewModel;
		private static TaskbarIcon _notifyIcon;

		public static void Configuration(TaskbarIcon notifyIcon, Zeta zeta) {
			_notifyIconViewModel = new NotifyIconViewModel();
			_notifyIcon = notifyIcon;
			_notifyIcon.DataContext = _notifyIconViewModel;
			_notifyIconViewModel.WakeUpApplication += (sender, args) => zeta.WakeUpApplication();
			_notifyIconViewModel.OpenSettingPanel += (sender, args) => zeta.OpenSettingWindow();
		}
	}
}