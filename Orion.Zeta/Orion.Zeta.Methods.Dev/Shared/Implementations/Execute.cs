using System;
using System.Diagnostics;

namespace Orion.Zeta.Methods.Dev.Shared.Implementations {
	public class Execute : IExecute {
		public string Program { get; set; }

		public string Parameters { get; set; }
		public void Start() {
			if (this.HaveParameters()) {
				Process.Start(this.Program, this.Parameters);
			}
			else {
				Process.Start(this.Program);
			}
		}

		private bool HaveParameters() {
			return !String.IsNullOrEmpty(this.Parameters);
		}
	}
}