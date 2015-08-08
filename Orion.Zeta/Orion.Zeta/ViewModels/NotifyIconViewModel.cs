using System;
using System.Windows;
using System.Windows.Input;
using Orion.Zeta.Methods.Ui.Dev;
using Orion.Zeta.Methods.Ui.Dev.Tools.MVVM;

namespace Orion.Zeta.ViewModels {
	public class NotifyIconViewModel : BaseViewModel {
		public ICommand WakeUpCommand { get; private set; }

		public ICommand ShutDownApplicationCommand { get; private set; }

		public ICommand OpenSettingCommand { get; private set; }

		public event EventHandler WakeUpApplication;

		public event EventHandler OpenSettingPanel;

		public string ToopTipText { get; private set; }

		public NotifyIconViewModel() {
			this.WakeUpCommand = new RelayCommand(this.OnWakeUpCommand);
			this.ShutDownApplicationCommand = new RelayCommand(this.OnShutDownApplicationCommand);
			this.OpenSettingCommand = new RelayCommand(this.OnOpenSettingCommand);
			this.ToopTipText = "Zeta - Alpha (Ctrl + Space)";
		}

		private void OnOpenSettingCommand() {
			this.OpenSettingPanel?.Invoke(this, EventArgs.Empty);
		}

		private void OnShutDownApplicationCommand() {
			Application.Current.Shutdown();
		}

		private void OnWakeUpCommand() {
			this.WakeUpApplication?.Invoke(this, EventArgs.Empty);
		}
	}
}