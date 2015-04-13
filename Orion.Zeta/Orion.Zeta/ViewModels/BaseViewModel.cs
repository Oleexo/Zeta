using System.ComponentModel;
using System.Runtime.CompilerServices;
using Orion.Zeta.Annotations;

namespace Orion.Zeta.ViewModels {
	public abstract class BaseViewModel : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			var handler = this.PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}