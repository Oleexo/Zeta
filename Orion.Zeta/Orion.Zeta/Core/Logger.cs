using System;
using log4net;

namespace Orion.Zeta.Core {
	public static class Logger {
		private static readonly ILog EventLogger = LogManager.GetLogger("RollingFile");

		public static void LogInfo(string message) {
			EventLogger.Info(message);
		}

		public static void LogWarning(string message) {
			EventLogger.Warn(message);
		}

		public static void LogError(string message) {
			EventLogger.Error(message);
		}

		public static void LogError(string message, Exception e) {
			EventLogger.Error(message, e);
		}

		public static void LogError(string message, params object[] args) {
			EventLogger.ErrorFormat(message, args);
		}

		public static void LogFatal(string message, Exception e) {
			EventLogger.Fatal(message, e);
		}

		public static void LogFatal(string message, params object[] args) {
			EventLogger.FatalFormat(message, args);
		}
	}
}