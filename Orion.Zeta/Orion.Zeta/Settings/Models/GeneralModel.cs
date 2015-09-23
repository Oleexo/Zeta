namespace Orion.Zeta.Settings.Models {
    public class GeneralModel {
        public GeneralModel() {
	        this.IsStartOnBoot = true;
	        this.IsAutoRefreshEnbabled = true;
	        this.AutoRefresh = 60;
        }
        public int AutoRefresh { get; set; }
        public bool IsAutoRefreshEnbabled { get; set; }
        public bool IsStartOnBoot { get; set; }
    }
}