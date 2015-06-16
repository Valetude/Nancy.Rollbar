namespace Nancy.Rollbar {
    public enum RollbarResponseCode {
        Success = 200,
        BadRequest = 400,
        Unauthorized = 401,
        AccessDenied = 403,
        RequestTooLarge = 413,
        UnprocessablePayload = 422,
        TooManyRequests = 429,
        InternalServerError = 500,
    }
}