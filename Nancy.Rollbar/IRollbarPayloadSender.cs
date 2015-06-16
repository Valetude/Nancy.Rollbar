using Valetude.Rollbar;

namespace Nancy.Rollbar {
    public interface IRollbarPayloadSender {
        RollbarResponse SendPayload(RollbarPayload payload);
    }
}