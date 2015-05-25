using Orion.Zeta.Core.Settings;

namespace Orion.Zeta.Core.SearchMethods {
    public abstract class SearchMethodContainerBase {
        public ISettingContainer SettingContainer;

        public bool IsModifiable() {
            return this.SettingContainer != null;
        }
    }
}