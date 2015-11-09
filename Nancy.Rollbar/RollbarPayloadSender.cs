using System;
using System.Net;
using Nancy.Rollbar.Api;
using Valetude.Rollbar;

namespace Nancy.Rollbar {
    public class RollbarPayloadSender : IRollbarPayloadSender {
        private const string RollbarApiEndpoint = "https://api.rollbar.com/api/1/item/";
        private const string ContentType = "application/json";

        public RollbarResponse SendPayload(RollbarPayload payload) {
            using (var client = new WebClient()) {
                client.Headers[HttpRequestHeader.ContentType] = ContentType;
                try {
                    return new RollbarResponse {
                        Message = client.UploadString(new Uri(RollbarApiEndpoint), "POST", payload.ToJson()),
                        StatusCode = RollbarResponseCode.Success,
                    };
                }
                catch (WebException exception) {
                    return new RollbarResponse {
                        Message = exception.Message,
                        StatusCode = ToRollbarStatus(exception.Status),
                    };
                }
            }
        }

        private static RollbarResponseCode ToRollbarStatus(WebExceptionStatus status) {
            switch ((int) status) {
                case 200:
                    return RollbarResponseCode.Success;
                case 400:
                    return RollbarResponseCode.BadRequest;
                case 401:
                    return RollbarResponseCode.Unauthorized;
                case 403:
                    return RollbarResponseCode.AccessDenied;
                case 413:
                    return RollbarResponseCode.RequestTooLarge;
                case 422:
                    return RollbarResponseCode.UnprocessablePayload;
                case 429:
                    return RollbarResponseCode.TooManyRequests;
                case 500:
                    return RollbarResponseCode.InternalServerError;
                default:
                    throw new ArgumentException("Invalid Status returned from Rollbar", "status");
            }
        }
    }
}
