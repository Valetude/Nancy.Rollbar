namespace Nancy.Rollbar.Api {
    public interface IHasAccessToken {
        string RollbarAccessToken { get; }
    }
}