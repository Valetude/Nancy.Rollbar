using System;
using Nancy.Rollbar.Api;
using Valetude.Rollbar;

namespace Nancy.Rollbar {
    public class RollbarPayloadFactory : IRollbarPayloadFactory {
        private readonly IHasAccessToken _hasAccessToken;
        private readonly IRollbarDataFactory _rollbarDataFactory;
        private readonly IRollbarDataScrubber _rollbarDataScrubber;

        public RollbarPayloadFactory(IHasAccessToken hasAccessToken, IRollbarDataFactory rollbarDataFactory, IRollbarDataScrubber rollbarDataScrubber) {
            _hasAccessToken = hasAccessToken;
            _rollbarDataFactory = rollbarDataFactory;
            _rollbarDataScrubber = rollbarDataScrubber;
        }

        public RollbarPayload GetPayload(NancyContext context, Exception e) {
            var accessToken = _hasAccessToken.RollbarAccessToken;
            var rollbarData = _rollbarDataFactory.GetData(context, e);
            return new RollbarPayload(accessToken, _rollbarDataScrubber.ScrubRollbarData(rollbarData));
        }
    }
}