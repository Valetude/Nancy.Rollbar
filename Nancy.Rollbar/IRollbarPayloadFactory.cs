using System;
using Valetude.Rollbar;

namespace Nancy.Rollbar {
    public interface IRollbarPayloadFactory {
        RollbarPayload GetPayload(NancyContext context, Exception e);
    }
}