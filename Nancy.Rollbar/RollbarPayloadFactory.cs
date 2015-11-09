using System;
using Nancy.Rollbar.Api;
using Valetude.Rollbar;

namespace Nancy.Rollbar {
    public class RollbarPayloadFactory : IRollbarPayloadFactory {
        private readonly IHasAccessToken _hasAccessToken;
        private readonly IRollbarDataFactory _rollbarDataFactory;

        public RollbarPayloadFactory(IHasAccessToken hasAccessToken, IRollbarDataFactory rollbarDataFactory) {
            _hasAccessToken = hasAccessToken;
            _rollbarDataFactory = rollbarDataFactory;
        }

        public RollbarPayload GetPayload(NancyContext context, Exception e) {
            var accessToken = _hasAccessToken.RollbarAccessToken;
            var rollbarData = _rollbarDataFactory.GetData(context, e);
            return new RollbarPayload(accessToken, rollbarData);
        }
    }
}