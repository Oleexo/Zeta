namespace Orion.Zeta.Settings.Models {
    public class GeneralModel {
        public GeneralModel() {
            this.IsAlwaysOnTop = true;
            this.IsHideWhenLostFocus = true;
        }
        public bool IsHideWhenLostFocus { get; set; }
        public bool IsAlwaysOnTop { get; set; }
        public int AutoRefresh { get; set; }
        public bool IsAutoRefreshEnbabled { get; set; }
        public bool IsStartOnBoot { get; set; }
    }
}