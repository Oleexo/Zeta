using Orion.Zeta.Methods.Dev;
using Orion.Zeta.ViewModels;

namespace Orion.Zeta.Core.Settings {
    public class StyleApplicable : IModifiableSettings {
        private readonly IModifiableStyleSetting _modifiableStyleSetting;

        public StyleApplicable(IModifiableStyleSetting modifiableStyleSetting) {
            this._modifiableStyleSetting = modifiableStyleSetting;
        }

        public void ApplyChanges(object item) {
        }
    }
}