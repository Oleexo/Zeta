using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.TeamFoundation.MVVM;

namespace Orion.Zeta.ViewModels {
	public class NotifyIconViewModel : BaseViewModel {
		public ICommand WakeUpCommand { get; set; }

		public ICommand ShutDownApplicationCommand { get; set; }

		public event EventHandler WakeUpApplication;

		public NotifyIconViewModel() {
			this.WakeUpCommand = new RelayCommand(this.OnWakeUpCommand);
			this.ShutDownApplicationCommand = new RelayCommand(this.OnShutDownApplicationCommand);
		}

		private void OnShutDownApplicationCommand() {
			Application.Current.Shutdown();
		}

		private void OnWakeUpCommand() {
			if (this.WakeUpApplication != null)
				this.WakeUpApplication(this, new EventArgs());
		}
	}
}