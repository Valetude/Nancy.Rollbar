using Valetude.Rollbar;

namespace Nancy.Rollbar.Api {
    public interface IRollbarPayloadSender {
        RollbarResponse SendPayload(RollbarPayload payload);
    }
}