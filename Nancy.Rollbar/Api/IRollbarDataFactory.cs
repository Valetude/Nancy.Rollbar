using System;
using Valetude.Rollbar;

namespace Nancy.Rollbar.Api {
    public interface IRollbarDataFactory {
        RollbarData GetData(NancyContext context, Exception exception);
    }
}