using System.Collections.Generic;
using System.Threading.Tasks;
using Orion.Zeta.Core.Settings;
using Orion.Zeta.Persistence;
using Orion.Zeta.Settings.Containers;

namespace Orion.Zeta.Services {
	public interface ISettingsService {
		ISettingRepository SettingRepository { get; }
		IEnumerable<ISettingContainer> GetSettingContainers();
		IEnumerable<ISettingContainer> GetGlobalSettingContainers();
		void RegisterGlobal(IApplicationSettingContainer settingContainer);
		void Register(ISettingContainer settingContainer);
		bool IsEnabled(string header);
		void SaveChanges();
		Task SaveChangesAsync();
		void ToggleMethod(string header, bool? value);
	}
}