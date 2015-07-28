using System;
using Orion.Zeta.Methods.Dev.Setting;

namespace Orion.Zeta.Services {
	public interface IApplicationSettingService : IDataService {
		event EventHandler Closing;

		void OnClosing();
	}
}