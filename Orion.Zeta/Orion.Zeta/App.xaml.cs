using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Hardcodet.Wpf.TaskbarNotification;
using NHotkey;
using NHotkey.Wpf;
using Orion.Zeta.ViewModels;

namespace Orion.Zeta
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
	    private static MainWindow _mainWindow {
		    get {
				return Current.MainWindow as MainWindow;
		    }
	    }

	    public static TaskbarIcon NotifyIcon { get; private set; }

		public static NotifyIconViewModel NotifyIconViewModel { get; private set; }

	    private void App_OnStartup(object sender, StartupEventArgs e) {
		    var process = Process.GetCurrentProcess();
		    if (Process.GetProcesses().Count(p => p.ProcessName.Equals(process.ProcessName)) > 1) {
			    Current.Shutdown();
			    return;
//			    var processes = Process.GetProcesses().Where(p => p.ProcessName.Equals(process.ProcessName));
		    }
			NotifyIcon = this.FindResource("TaskbarIcon") as TaskbarIcon;
		    if (NotifyIcon == null) {
				Current.Shutdown();
			    return;
		    }
			NotifyIconViewModel = new NotifyIconViewModel();
		    NotifyIcon.DataContext = NotifyIconViewModel;
			NotifyIconViewModel.WakeUpApplication += this.NotifyIconViewModelOnWakeUpApplication;
			HotkeyManager.Current.AddOrReplace("LaunchZeta", Key.Space, ModifierKeys.Control, this.OnWakeUpApplication);
	    }

	    private void NotifyIconViewModelOnWakeUpApplication(object sender, EventArgs eventArgs) {
		    _mainWindow.WakeUpApplication();
	    }

	    private void OnWakeUpApplication(object sender, HotkeyEventArgs e) {
			_mainWindow.WakeUpApplication();
		}
    }
}
