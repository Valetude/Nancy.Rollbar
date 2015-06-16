namespace Nancy.Rollbar {
    public interface IHasAccessToken {
        string RollbarAccessToken { get; }
    }
}