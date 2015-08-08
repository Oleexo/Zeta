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
				this.GenerateProcessStartInfo();
			if (this._processInfo == null) {
				throw new Exception();
			}
			Process.Start(this._processInfo);
		}

		private void GenerateProcessStartInfo() {
			if (this.Program == null || !File.Exists(this.Program))
				throw new Exception();
			this._processInfo = new ProcessStartInfo {
				FileName = Path.GetFileName(this.Program),
				WorkingDirectory = Path.GetDirectoryName(this.Program)
			};
			if (this.HaveParameters()) {
				this._processInfo.Arguments = this.Parameters;
			}
		}

		private bool HaveParameters() {
			return !String.IsNullOrEmpty(this.Parameters);
		}
	}
}