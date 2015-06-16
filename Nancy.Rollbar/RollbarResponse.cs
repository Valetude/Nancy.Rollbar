namespace Nancy.Rollbar {
    public class RollbarResponse {
        public RollbarResponseCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}