namespace Nancy.Rollbar.Tests {
    public class FakeAccessToken : IHasAccessToken {
        public string RollbarAccessToken {
            get {
                // Replace me with your own private token to run these tests.
                // Don't commit your token. That'd be no fun.
                return "A Fake Token (Sorry Rollbar!)";
            }
        }
    }
}
