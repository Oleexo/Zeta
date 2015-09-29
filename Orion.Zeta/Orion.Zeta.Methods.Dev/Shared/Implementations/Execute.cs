using System;
using System.Diagnostics;
using System.IO;

namespace Orion.Zeta.Methods.Dev.Shared.Implementations {
	public class Execute : IExecute {
		public string Program { get; set; }

		public string Parameters { get; set; }

		private ProcessStartInfo _processInfo;

		public void Start() {
			if (_processInfo == null)
				GenerateProcessStartInfo();
			if (_processInfo == null) {
				throw new Exception();
			}
			Process.Start(_processInfo);
		}

		private void GenerateProcessStartInfo() {
			if (Program == null || !File.Exists(Program))
				throw new Exception();
			_processInfo = new ProcessStartInfo {
				FileName = Path.GetFileName(Program),
				WorkingDirectory = Path.GetDirectoryName(Program)
			};
			if (HaveParameters()) {
				_processInfo.Arguments = Parameters;
			}
		}

		private bool HaveParameters() {
			return !string.IsNullOrEmpty(Parameters);
		}
	}
}