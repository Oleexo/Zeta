using System;

namespace Orion.Zeta.Methods.ApplicationSearch {
	public class PathUnRegisteredException : Exception {
		public PathUnRegisteredException(string path) : base(path) {
		}
	}
}