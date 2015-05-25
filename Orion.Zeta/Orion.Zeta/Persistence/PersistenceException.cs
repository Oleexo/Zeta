using System;

namespace Orion.Zeta.Persistence {
    public class PersistenceException : Exception {
        public PersistenceException(string message) : base(message) {
        }
    }
}