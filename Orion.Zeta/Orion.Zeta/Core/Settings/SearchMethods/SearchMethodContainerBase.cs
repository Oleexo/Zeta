namespace Orion.Zeta.Core.Settings.SearchMethods {
    public abstract class SearchMethodContainerBase {
        public ISettingContainer SettingContainer;

        public bool IsModifiable() {
            return this.SettingContainer != null;
        }
    }
}