using System;

namespace Nancy.Rollbar.Tests {
    public class FakeException : Exception {
        public FakeException(string message) : base(message) {
        }
    }
}