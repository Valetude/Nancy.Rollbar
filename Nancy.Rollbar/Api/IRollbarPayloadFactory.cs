using System;
using Valetude.Rollbar;

namespace Nancy.Rollbar.Api {
    public interface IRollbarPayloadFactory {
        RollbarPayload GetPayload(NancyContext context, Exception e);
    }
}