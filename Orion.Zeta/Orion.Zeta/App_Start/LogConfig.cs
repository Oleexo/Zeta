namespace Orion.Zeta {
	public static class LogConfig {
		public static void Configure() {
#if DEBUG
			log4net.Config.XmlConfigurator.Configure();
#endif
		}
	}
}