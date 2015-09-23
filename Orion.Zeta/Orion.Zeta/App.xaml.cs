using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace Orion.Zeta {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
	    private static Zeta _zeta;

        private void App_OnStartup(object sender, StartupEventArgs e) {
#if !(DEBUG)
			Logger.LogInfo("Application Start");
			var process = Process.GetCurrentProcess();
			if (Process.GetProcesses().Count(p => p.ProcessName.Equals(process.ProcessName)) > 1) {
				Current.Shutdown();
				return;
			}
#endif
			_zeta = new Zeta(this);
	        _zeta.Start();
			LogConfig.Configure();
			var notifyIcon = this.FindResource("TaskbarIcon") as TaskbarIcon;
            if (notifyIcon == null) {
                Current.Shutdown();
                return;
            }
			NotifyIconConfig.Configuration(notifyIcon, _zeta);
			HotKeyConfig.Configuration(_zeta);
        }
    }
}
