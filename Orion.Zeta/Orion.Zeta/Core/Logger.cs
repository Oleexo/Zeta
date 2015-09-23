using System;
using log4net;

namespace Orion.Zeta.Core {
	public class Logger {

		#region Static Fields

		private static readonly ILog RootLogger = LogManager.GetLogger("RootLogger");
		private static Logger _instance;

		public static Logger Instance => _instance ?? (_instance = new Logger());

		#endregion

		#region Public Methods and Operators

		public void LogDebug(string message) {
			RootLogger.Debug(message);
		}

		public void LogError(string message) {
			RootLogger.Error(message);
		}

		public void LogError(string message, Exception e) {
			RootLogger.Error(message, e);
		}

		public void LogError(string message, params object[] args) {
			RootLogger.ErrorFormat(message, args);
		}

		public void LogFatal(string message, Exception e) {
			RootLogger.Fatal(message, e);
		}

		public void LogFatal(string message, params object[] args) {
			RootLogger.FatalFormat(message, args);
		}

		public void LogInfo(string message) {
			RootLogger.Info(message);
		}

		public void LogWarning(string message) {
			RootLogger.Warn(message);
		}

		#endregion
	}
}