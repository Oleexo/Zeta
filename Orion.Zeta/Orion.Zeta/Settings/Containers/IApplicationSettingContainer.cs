using Orion.Zeta.Core.Settings;

namespace Orion.Zeta.Settings.Containers {
	public interface IApplicationSettingContainer : ISettingContainer {
		void ApplyConfiguration();
	}
}