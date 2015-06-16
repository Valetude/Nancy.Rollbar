using System;
using Valetude.Rollbar;

namespace Nancy.Rollbar {
    public interface IRollbarDataFactory {
        RollbarData GetData(NancyContext context, Exception exception);
    }
}