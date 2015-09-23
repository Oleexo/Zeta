using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Orion.Zeta.Launcher {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		private void App_OnStartup(object sender, StartupEventArgs e) {
			var currentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
			if (currentPath == null) {
				Current.Shutdown();
				return;
			}
			var executablePath = Path.Combine(currentPath, ConfigurationManager.AppSettings["applicationExe"]);
			Process.Start(executablePath);
			Current.Shutdown();
		}
	}
}
